# This is a sample Python script.
import time
import re
import datetime

from selenium import webdriver
from selenium.webdriver.support.ui import Select
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import locale
# Set the locale to Russian (Russia)
locale.setlocale(locale.LC_ALL, 'ru_RU.UTF-8')

web = webdriver.Chrome()
web.get('http://192.168.1.64/bulgaria/index.html')
wait = WebDriverWait(web, 1)
wait2 = WebDriverWait(web, 2)
time.sleep(2)

# reservation date
dt = web.find_element('xpath', '//*[@id="bgSubmissionDocForm"]/div/h4[3]/font/font')
date_time_str = dt.text.split(': ')[1]
date_time = datetime.datetime.strptime(date_time_str, "%d/%m/%Y %H:%M:%S")
formattedDt = date_time.strftime("%d.%m.%Y г. %H:%M:%S")
print(formattedDt)  #11.09.2023 г. 16:15:00
element = web.find_element('xpath', '//*[@id="ReservedDate"]')
element.send_keys(formattedDt)
# will be implemented

# name area
usrName = 'Ramazan Baybörek'
name = web.find_element('xpath', '//*[@id="ApplicantName"]')
name.send_keys(usrName)

# birth date area
day = 11    # 10
option_xpath = f'//*[@id="bgSubmissionDocForm"]/div/div[4]/div/select[1]/option[{day}]'
option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
option_element.click()
month = 4 # march
option_xpath = f'//*[@id="bgSubmissionDocForm"]/div/div[4]/div/select[2]/option[{month}]'
option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
option_element.click()
year = 86 # 1997
option_xpath = f'//*[@id="bgSubmissionDocForm"]/div/div[4]/div/select[3]/option[{year}]'
option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
option_element.click()

# passport area
usrPassport = 'U123456789'
passport = web.find_element('xpath', '//*[@id="ApplicantPassport"]')
passport.send_keys(usrPassport)

# passport file area
# will be implemented

# email area
usrEmail = 'ramazanbayborek@gmail.com'
email = web.find_element('xpath', '//*[@id="Email"]')
email.send_keys(usrEmail)


# citizenship area
ctz = 208    # türkiye
option_xpath = f'//*[@id="ResidentCountryID"]/option[{ctz}]'
option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
option_element.click()

# reason area
option_xpath = '//*[@id="RegForSubmissionSubjectID"]/option[2]'
option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
option_element.click()

# fullname = 'Deneme Hesaoasdaskdas'
# name = web.find_element('xpath', '//*[@id="fullname"]')
# name.send_keys(fullname)

# fls = 'DH'
# firstletters = web.find_element('xpath', '//*[@id="firstLetters"]')
# firstletters.send_keys(fls)
#
# uni = 0
# university_dropdown = wait.until(EC.element_to_be_clickable((By.XPATH, '//*[@id="mat-select-0"]')))
# university_dropdown.click()
# option_xpath = f'//*[@id="mat-option-{uni}"]'
# option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
# option_element.click()
#
# dep = 4
# department_dropdown = wait.until(EC.element_to_be_clickable((By.XPATH, '//*[@id="mat-select-2"]')))
# department_dropdown.click()
# option_xpath = f'//*[@id="mat-option-{dep}"]'
# option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
# option_element.click()
#
# sno = '20170601042'
# studentno = web.find_element('xpath', '//*[@id="studentNo"]')
# studentno.send_keys(sno)
#
# useremail = 'deneme@deneme.com'
# email = web.find_element('xpath', '//*[@id="email"]')
# email.send_keys(useremail)
#
# userphone = '5060526793'
# phone = web.find_element('xpath', '//*[@id="phone"]')
# phone.send_keys(userphone)
#
# pwd = '123456'
# password = web.find_element('xpath', '//*[@id="password"]')
# password.send_keys(pwd)
#
# # select = web.find_element('xpath', '//*[@id="agreements"]/label/span[1]')
# aggrements = wait.until(EC.element_to_be_clickable((By.XPATH, '//*[@id="agreements"]/label/span[1]')))
# web.execute_script("arguments[0].scrollIntoView();", aggrements)
# aggrements.click()

input("Press Enter when you're ready to click the button...")


# print("Before clicking submit")
# form = wait.until(EC.element_to_be_clickable((By.XPATH, '/html/body/app-root/layout/empty-layout/div/div/auth-sign-up/div/div[1]/div/form')))
# submitbtn = form.find_element(By.XPATH, '//button')
# submitbtn.click()
# form.submit()
# register = web.find_element('xpath', '/html/body/app-root/layout/empty-layout/div/div/auth-sign-up/div/div[1]/div/form/button/span[1]/span')
# register.submit()
# button = wait2.until(EC.element_to_be_clickable((By.XPATH, '//button[contains(text(), "Kaydol")]')))
# button.click()
# submit = wait.until(EC.element_to_be_clickable((By.XPATH, '/html/body/app-root/layout/empty-layout/div/div/auth-sign-up/div/div[1]/div/form/button/span[1]/span')))
# submit.click()

# print("After clicking submit")

# Press the green button in the gutter to run the script.
# if __name__ == '__main__':
# See PyCharm help at https://www.jetbrains.com/help/pycharm/
