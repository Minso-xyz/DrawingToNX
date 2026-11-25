from image_processor import (load_image, convert_to_grayscale, create_binary_image)
from geometry_detector import find_contours
import cv2

def main():
	image = load_image("Input/plate.jpg")
	gray = convert_to_grayscale(image)
	binary = create_binary_image(gray)
	contours, hierarchy = find_contours(binary)

	print("Original: ", image.shape)
	print("Grayscale: ", gray.shape)
	print("Number of contours: ", len(contours))

	cv2.imwrite("Output/refactor_gray.png", gray)
	cv2.imwrite("Output/refactor_binary.png", binary)
	print("Image preprocessing completed.")

# execute main()
if __name__ == "__main__":
    main()