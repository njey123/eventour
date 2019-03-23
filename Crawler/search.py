import time 
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

def getCityUrl(city):

    driver = webdriver.Chrome('/usr/local/bin/chromedriver')
    driver.implicitly_wait(10) # this lets webdriver wait 10 seconds for the website to load
    driver.get("https://www.tripadvisor.ca/")
    driver.refresh()
    button = driver.find_element_by_class_name('attractions')
    button.click()

    text_box = driver.find_element_by_class_name('input-text-input-ManagedTextInput__managedInput--2RESp') # input selector
    text_box.send_keys(city) # enter text in input
    time.sleep(1)
    text_box.send_keys(Keys.RETURN) # click the submit button
    wait2 = WebDriverWait(driver, 10)
    wait2.until(EC.title_contains(city))
    city_url = driver.current_url
    see_more = driver.find_element_by_class_name('attractions-attraction-overview-main-TopPOIs__see_more--2Vsb-')
    see_more.click()
    time.sleep(2)
    next_button = driver.find_element_by_link_text('Next')
    next_button.click()
    time.sleep(2)
    next_url = driver.current_url

    urls = [city_url,next_url]

    driver.quit()
    return urls