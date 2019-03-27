import time 
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
#from pyvirtualdisplay import Display

#import json

options = Options()
#options.add_argument("--no-sandbox")
options.add_argument("--headless")


def getAttractionDescription(url):
    #driver = webdriver.Chrome('/usr/local/bin/chromedriver',chrome_options=options)
    driver = webdriver.Chrome('/usr/bin/chromedriver', chrome_options=options)
    driver.implicitly_wait(10) # this lets webdriver wait 10 seconds for the website to load
    driver.get(url)
    #driver.get('https://www.tripadvisor.ca/Attraction_Review-g60763-d105127-Reviews-Central_Park-New_York_City_New_York.html')
    
    button = driver.find_element_by_class_name('attractions-attraction-detail-about-card-Description__readMore--2pd33')
    button.click()
    time.sleep(2)
    #wait.until(EC.presence_of_element_located((By.CLASS_NAME, "overlays-pieces-Overlay__overlay--1Lqh_")))
    description = driver.find_element_by_class_name('attractions-attraction-detail-about-card-Description__modalText--1oJCY').text
    driver.quit()
    return description