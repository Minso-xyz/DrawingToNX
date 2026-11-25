import cv2

def find_contours(binary_image):
	contours, hierarchy = cv2.findContours(
		binary_image,
		cv2.RETR_TREE,
		cv2.CHAIN_APPROX_SIMPLE
	)
	return contours, hierarchy