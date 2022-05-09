import requests
import cv2
import numpy as np
import imutils

url = "http://192.168.31.41:8080/shot.jpg"

while True:
    img_resp = requests.get(url)
    print(img_resp.status_code)
    img_arr = np.array(bytearray(img_resp.content), dtype=np.uint8)
    img = cv2.imdecode(img_arr, -1)
    img = imutils.resize(img, width=1000, height=1000)
    cv2.imshow("Android_Cam", img)
    if cv2.waitKey(1) == 27:
        break

cv2.destroyAllWindows()