import cv2

def find_contours(binary_image):
	contours, hierarchy = cv2.findContours(
		binary_image,
		cv2.RETR_TREE,
		cv2.CHAIN_APPROX_SIMPLE
	)
	return contours, hierarchy

def find_largest_contour(contours):
	largest_contour = max(contours, key=cv2.contourArea)
	largest_area = cv2.contourArea(largest_contour)
	return largest_contour, largest_area

def draw_contour(image, contour, result_path):
	result = image.copy()
	cv2.drawContours(
		result,   # where to draw
		[contour],   # what to draw (list)
		-1,   # draw all the contour received
		(0,0,255),  # Blue = 0, Green = 0, Red=255
		3   # The thickness of line [px]
	)
	cv2.imwrite(result_path, result)
	return result

def check_boundingbox(largest_contour):
	x,y,width,height = cv2.boundingRect(largest_contour)
	return x, y, width, height
