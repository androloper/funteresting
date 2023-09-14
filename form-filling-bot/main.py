# This is a sample Python script.
import time
from selenium import webdriver
from selenium.webdriver.support.ui import Select
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.

web = webdriver.Chrome()
web.get('https://notumburada.com/#/kaydol')
wait = WebDriverWait(web, 1)
wait2 = WebDriverWait(web, 2)
time.sleep(3)

fullname = 'Deneme Hesaoasdaskdas'
name = web.find_element('xpath', '//*[@id="fullname"]')
name.send_keys(fullname)

fls = 'DH'
firstletters = web.find_element('xpath', '//*[@id="firstLetters"]')
firstletters.send_keys(fls)

uni = 0
# university = web.find_elements('xpath', '//*[@id="mat-select-0-panel"]')
# university_dropdown = Select(university[uni])
# uni_xpath = f'//*[@id="mat-select-{uni}"]'
university_dropdown = wait.until(EC.element_to_be_clickable((By.XPATH, '//*[@id="mat-select-0"]')))
university_dropdown.click()
option_xpath = f'//*[@id="mat-option-{uni}"]'
option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
option_element.click()

dep = 4
department_dropdown = wait.until(EC.element_to_be_clickable((By.XPATH, '//*[@id="mat-select-2"]')))
department_dropdown.click()
option_xpath = f'//*[@id="mat-option-{dep}"]'
option_element = wait.until(EC.element_to_be_clickable((By.XPATH, option_xpath)))
option_element.click()

# department = web.find_elements('xpath', '//*[@id="mat-select-value-3"]')
# department_dropdown = Select(department[dep])
# department_dropdown.select_by_index(dep)

sno = '20170601042'
studentno = web.find_element('xpath', '//*[@id="studentNo"]')
studentno.send_keys(sno)

useremail = 'deneme@deneme.com'
email = web.find_element('xpath', '//*[@id="email"]')
email.send_keys(useremail)

userphone = '5060526793'
phone = web.find_element('xpath', '//*[@id="phone"]')
phone.send_keys(userphone)

pwd = '123456'
password = web.find_element('xpath', '//*[@id="password"]')
password.send_keys(pwd)

# select = web.find_element('xpath', '//*[@id="agreements"]/label/span[1]')
aggrements = wait.until(EC.element_to_be_clickable((By.XPATH, '//*[@id="agreements"]/label/span[1]')))
web.execute_script("arguments[0].scrollIntoView();", aggrements)
aggrements.click()

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
