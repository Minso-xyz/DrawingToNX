from image_processor import (load_image, convert_to_grayscale, create_binary_image)
from geometry_detector import find_contours
from geometry_detector import find_largest_contour
from geometry_detector import draw_contour
from geometry_detector import check_boundingbox
import cv2

def main():
	image = load_image("Input/plate.jpg")
	gray = convert_to_grayscale(image)
	binary = create_binary_image(gray)
	contours, hierarchy = find_contours(binary)
	largestContour, largestArea = find_largest_contour(contours)
	x, y, width, height = check_boundingbox(largestContour)

	print("Original: ", image.shape)
	print("Grayscale: ", gray.shape)
	print("Number of contours: ", len(contours))
	print("Largest contour area: ", largestArea)
	print("X: ", x, " / Y: ", y, " Width: ", width, " / Height: ", height)

	cv2.imwrite("Output/refactor_gray.png", gray)
	cv2.imwrite("Output/refactor_binary.png", binary)
	draw_contour(image, largestContour, "Output/largest_contour.jpg")

	print("Image preprocessing completed.")

# execute main()
if __name__ == "__main__":
    main()