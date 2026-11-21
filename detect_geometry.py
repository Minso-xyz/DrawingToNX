import cv2
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