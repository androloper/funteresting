import cv2
import numpy as np

# for reading image
img = cv2.imread("rb.png")

# for edges
gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
gray = cv2.medianBlur(gray, 5)
edges = cv2.adaptiveThreshold(gray, 255, cv2.ADAPTIVE_THRESH_MEAN_C, cv2.THRESH_BINARY, 9, 9)

# cartoonization
color = cv2.bilateralFilter(img, 9, 250, 259)
cartoon = cv2.bitwise_and(color, color, mask=edges)

cv2.imshow("Image", img)  # for photo initial
cv2.imshow("edges", edges)  # for edge version
cv2.imshow("Cartoon", cartoon)  # for cartoon version
cv2.waitKey(0)
cv2.destroyAllWindows()
