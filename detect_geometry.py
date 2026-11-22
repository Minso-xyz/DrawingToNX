import cv2
import numpy as np
import pytesseract
pytesseract.pytesseract.tesseract_cmd = (r"C:\Program Files\Tesseract-OCR\tesseract.exe")

image = cv2.imread("Input/plate.jpg")
gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

_, binary = cv2.threshold(
	gray,
	200,
	255,cv2.THRESH_BINARY_INV
)

#print(image)
print("Original: ", image.shape)
print("Grayscale: ", gray.shape)

cv2.imwrite("Output/gray.jpg", gray)
cv2.imwrite("Output/binary.png", binary)

# print the color value of the binary image [0 255 (black/white)]
import numpy as np
print("Binary values: ", np.unique(binary))

# find contours of the binary image with the options (RETR_TREE, CHAIN_APPROX_SIMPLE)
contours, hierarchy = cv2.findContours(
	binary,
	cv2.RETR_TREE,
	cv2.CHAIN_APPROX_SIMPLE
)

# print the number of contours/area of the contour[0]
print("Number of contours: ", len(contours))
print(contours[0])
area = cv2.contourArea(contours[0])
print("Area: ", area)

# print the area of all the contours
for contour in contours:
	area = cv2.contourArea(contour)
	area = cv2.contourArea(contour)
	print(area)

# Retrieve the contour with the largest area
largest_contour = max(contours, key=cv2.contourArea)
largest_area = cv2.contourArea(largest_contour)
print("Largest contour area: ", largest_area)

# draw the result
result = image.copy()
cv2.drawContours(
	result,   # where to draw
	[largest_contour],   # what to draw (list)
	-1,   # draw all the contour received
	(0,0,255),  # Blue = 0, Green = 0, Red=255
	3   # The thickness of line [px]
)
cv2.imwrite("Output/largest_contour.jpg", result)

# check the boundingbox surrounding the largest_contour
x, y, width, height = cv2.boundingRect(largest_contour)
print("x: ", x)
print("y: ", y)
print("width: ", width)
print("height: ", height)

# crop the ROI part from the image (plate part)
roi = image[175:430, 115:430]
cv2.imwrite("Output/roi.jpg", roi)

# generate the binary image of the ROI
roi_gray = cv2.cvtColor(
    roi,
    cv2.COLOR_BGR2GRAY
)

_, roi_binary = cv2.threshold(
    roi_gray,
    200,
    255,
    cv2.THRESH_BINARY_INV
)

cv2.imwrite(
    "Output/roi_binary.png",
    roi_binary
)

# Retrieve the contours
roi_contours, roi_hierarchy = cv2.findContours(
    roi_binary,
    cv2.RETR_TREE,
    cv2.CHAIN_APPROX_SIMPLE
)
print("Number of ROI contours:", len(roi_contours))

# find the closed & bigger contours
for contour in roi_contours:
    area = cv2.contourArea(contour)
    if area < 1000:   # skip the small contours
        continue

# find the closed contours and calculate perimeter
    perimeter = cv2.arcLength(contour, True)
    approx = cv2.approxPolyDP(
        contour,
        0.02 * perimeter,   # 0.02 tolerance to simplify
        True
    )
    print("Area:", area, "/ Vertices:", len(approx))

    # result 
    #Area: 79755.5 / Vertices: 4
    #Area: 3918.5 / Vertices: 6
    #Area: 65272.5 / Vertices: 12
    #Area: 1311.5 / Vertices: 9
    #Area: 3360.5 / Vertices: 5

    # Draw - Area: 79755.5 / Vertices: 4
    if len(approx) == 4:
        plate_contour = approx
        print("Plate candidate area:", area)
        break

plate_result = roi.copy()
cv2.drawContours(
    plate_result,
    [plate_contour],
    -1,
    (0, 255, 0),
    3
)
cv2.imwrite("Output/plate_detected.jpg",plate_result)

# Hole detection
blurred = cv2.medianBlur(roi_gray, 5)  # optimize the noise
circles = cv2.HoughCircles(
    blurred,  # where to find
    cv2.HOUGH_GRADIENT,   # which Hough method to use
    dp=1,
    minDist=50,   # the centers of the retrived circles should be apart 50px at least
    param1=100,
    param2=30,
    minRadius=10,   # ignore the circles smaller than r=10
    maxRadius=30   # ignore the circles bigger than r=30
)

if circles is not None:
    print("Number of circles: ", len(circles[0]))   # circles are saved in circles[0]
else:
    print("No circles detected!")

    # draw the detected circle
circles = np.round(circles[0, :]).astype("int")
hole_result = roi.copy()

for x, y, radius in circles:
    # perimeter of the circle
    cv2.circle(
        hole_result,
        (x, y),
        radius,
        (0, 255, 0),
        2
    )

    # center of the circle
    cv2.circle(
        hole_result,
        (x, y),
        3,
        (0, 0, 255),
        -1
    )
    print("Center:", x, y, "Radius:", radius)   # print the center & radius of the detected circles

cv2.imwrite("Output/holes_detected.jpg",hole_result)

# read the dimensional data from the scaled image
ocr_image = cv2.resize(
    image,
    None,
    fx=3,
    fy=3,
    interpolation=cv2.INTER_CUBIC
)

# read the strings
print("Horizontal OCR result: ")
cv2.imwrite("Output/ocr_image.png", ocr_image)
text = pytesseract.image_to_string(
    ocr_image,
    config="--psm 11"  # find the text located randomly
)

# read the data
data = pytesseract.image_to_data(
    ocr_image,
    config="--psm 11",
    output_type=pytesseract.Output.DICT
)
for i in range(len(data["text"])):

    text_value = data["text"][i]
    confidence = int(data["conf"][i])

    if text_value.strip() != "" and confidence >= 80:  # show the text/data that is not empty/confidence >=80
        print(
            "Text:", text_value,
            "x:", data["left"][i],
            "y:", data["top"][i],
            "confidence:", confidence
        )
# result
#Text: 10 x: 394 y: 312 confidence: 96
#Text: 30 x: 779 y: 306 confidence: 96
#Text: 10 x: 1147 y: 312 confidence: 96
#Text: 5.5 x: 801 y: 1375 confidence: 90

# rotate the image at 90 degree and retrieve the vertical dimensions
vertical_roi = image[150:450, 0:130]
cv2.imwrite("Output/vertical_roi.jpg", vertical_roi)

rotated_vertical = cv2.rotate(vertical_roi, cv2.ROTATE_90_CLOCKWISE)
cv2.imwrite("Output/rotated_vertical.jpg",rotated_vertical)

vertical_text = pytesseract.image_to_string(
    rotated_vertical,
    config="--psm 11"
)

vertical_ocr_image = cv2.resize(
    rotated_vertical,
    None,
    fx=3,
    fy=3,
    interpolation=cv2.INTER_CUBIC
)

cv2.imwrite(
    "Output/vertical_ocr_image.png",
    vertical_ocr_image
)

vertical_data = pytesseract.image_to_data(
    vertical_ocr_image,
    config="--psm 11",
    output_type=pytesseract.Output.DICT
)

print("Vertical OCR Result:")

for i in range(len(vertical_data["text"])):

    text_value = vertical_data["text"][i]
    confidence = int(vertical_data["conf"][i])

    if text_value.strip() != "" and confidence >= 80:
        print(
            "Text:", text_value,
            "x:", vertical_data["left"][i],
            "y:", vertical_data["top"][i],
            "confidence:", confidence
        )