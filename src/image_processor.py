import cv2

# load image
def load_image(image_path):
	image = cv2.imread(image_path)

	if image is None:
		raise RuntimeError(f"Could not load image: {image_path}")
	return image

# convert the image to grayscale
def convert_to_grayscale(image):
	gray = cv2.cvtColor(image,cv2.COLOR_BGR2GRAY)
	return gray

# create the image to binary
def create_binary_image(gray):
	_, binary = cv2.threshold(gray, 200, 255, cv2.THRESH_BINARY_INV)
	return binary