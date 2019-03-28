import scrapy
#from search import getCityUrl
from description import getAttractionDescription
from scrapy.crawler import CrawlerProcess
from twisted.internet import reactor, defer
from scrapy.crawler import CrawlerRunner
from scrapy.utils.log import configure_logging
from items import AttractionItem
import json, sys, os
from termcolor import colored

class TripAdvisorSpider(scrapy.Spider):
    name = 'trip_advisor_spider'
    
    #urls = getCityUrl('New York') #get url for the city attraction search result page and the next page
    #start_urls = [urls[0]] #use the first url as start url

    def __init__(self, city = None, country = None, url = None, url_next = None, **kwargs):
        print(colored('spider initialized', 'cyan'))
        self.dest = city + ', ' + country
        #self.urls = getCityUrl(city, country)
        self.urls = [url, url_next]
        self.start_urls = [self.urls[0]]
        #self.start_urls.append('https://www.tripadvisor.ca/Attractions-g60763-Activities-New_York_City_New_York.html')
    
    def parse(self, response):
        SET_SELECTOR = '.attractions-attraction-overview-main-TopPOIs__item--1hjCB'
        for attraction in response.css(SET_SELECTOR):
            DETAIL_PAGE_SELECTOR = '.attractions-attraction-overview-main-TopPOIs__name--GndbY'
            detail_page = attraction.css(DETAIL_PAGE_SELECTOR).xpath('@href').extract_first()
            detail_page = ''.join(detail_page)
            if detail_page:
                yield scrapy.Request(
                    response.urljoin(detail_page), 
                    callback = self.parse_detail_page
                )  
        
        next_url = ''.join(self.urls[1])
        yield scrapy.Request(
            next_url, self.parse_next_page
        )
        
        
        
    def parse_next_page(self, response):
        SET_SELECTOR = '.listing_details'
        for attraction in response.css(SET_SELECTOR):
            DETAIL_PAGE_SELECTOR = '.listing_title'
            detail_page = attraction.css(DETAIL_PAGE_SELECTOR).xpath('a/@href').extract_first()
            detail_page = ''.join(detail_page)
            if detail_page:
                yield scrapy.Request(
                    response.urljoin(detail_page), 
                    callback = self.parse_detail_page
                )  
        
        NEXT_PAGE_SELECTOR = '.next'
        next_page = response.css(NEXT_PAGE_SELECTOR).xpath('@href').extract()
        next_page = ''.join(next_page)
        if next_page:
            yield scrapy.Request(
                response.urljoin(next_page), 
                callback=self.parse_next_page
            )  
        
    
    def parse_detail_page(self, response):
        NAME_SELECTOR = '.ui_header ::text'
        RATING_SELECTOR = '.prw_common_bubble_rating'
        REVIEW_COUNT_SELECTOR = '.reviewCount ::text'
        IMAGE_SELECTOR = '.basicImg'
        DURATION_SELECTOR = '.attractions-attraction-detail-about-card-AboutSection__sectionWrapper--3PMQg ::text'
        STREET_ADDRESS_SELECTOR = '.street-address ::text'
        EXTENDED_ADDRESS_SELECTOR = '.extended-address ::text'
        LOCALITY_SELECTOR = '.locality ::text'
        item = AttractionItem()
        item['name'] = response.css(NAME_SELECTOR).extract_first()
        item['rating'] = response.css(RATING_SELECTOR).xpath('span/@class').extract_first()
        item['review_count'] = response.css(REVIEW_COUNT_SELECTOR).extract_first()
        item['image_url'] = response.css(IMAGE_SELECTOR).xpath("@src").extract_first()
        item['duration'] = response.css(DURATION_SELECTOR).extract()
        # combine address parts into one item, remove any NoneType
        if(response.css(STREET_ADDRESS_SELECTOR).extract_first()):
            street_address = ''.join(response.css(STREET_ADDRESS_SELECTOR).extract_first())+', '
        else:
            street_address = ''
        if(response.css(EXTENDED_ADDRESS_SELECTOR).extract_first()):
            extended_address = ''.join(response.css(EXTENDED_ADDRESS_SELECTOR).extract_first())+', '
        else:
            extended_address = ''
        if(response.css(LOCALITY_SELECTOR).extract_first()):
            locality = ''.join(response.css(LOCALITY_SELECTOR).extract_first())
        else:
            locality = ''
        address = street_address + extended_address + locality
        item['address'] = [address]

        # get attraction description using Selenium
        if(response.css('.attractions-attraction-detail-about-card-Description__readMore--2pd33').extract()):
            item['description'] = [getAttractionDescription(response.request.url)]
            print(item['description'])
        else:
            if(response.css('.attractions-attraction-detail-about-card-AttractionDetailAboutCard__section--1_Efg').xpath('span').extract()):
                item['description'] = response.css('.attractions-attraction-detail-about-card-AttractionDetailAboutCard__section--1_Efg ::text').extract_first()
            else:
                item['description'] = ''
        
        
        yield item
'''
process = CrawlerProcess({
    'USER_AGENT': 'Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)',
    'ITEM_PIPELINES' : {'pipeline.ItemPipeline':100}
    })   
'''
'''
with open('cities.json') as f:
    cities = json.load(f)

for city in cities:
    process = CrawlerProcess({
    'USER_AGENT': 'Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)',
    'ITEM_PIPELINES' : {'jsonpipeline.JsonPipeline':100}
    }) 
    process.crawl(TripAdvisorSpider, city = city['city'], country = city['country'])

process.start() 
'''

'''
process = CrawlerProcess({
    'USER_AGENT': 'Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)',
    'ITEM_PIPELINES' : {'jsonpipeline.JsonPipeline':100}
    })  
process.crawl(TripAdvisorSpider, city = "New York City", country = "USA", url = "https://www.tripadvisor.ca/Attractions-g60763-Activities-New_York_City_New_York.html", url_next = "https://www.tripadvisor.ca/Attractions-g60763-Activities-oa30-New_York_City_New_York.html")
process.start() 
'''
configure_logging()
runner = CrawlerRunner({
    'USER_AGENT': 'Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)',
    #'ITEM_PIPELINES' : {'jsonpipeline.JsonPipeline':100}
    'ITEM_PIPELINES' : {'pipeline.ItemPipeline':100}
    })

@defer.inlineCallbacks
def crawl():
    print(os.path.abspath('cities.json'))
    print(os.getcwd())
    with open('/home/alien/crawler/cities.json') as f:
        cities = json.load(f)
    for city in cities:
        yield runner.crawl(TripAdvisorSpider,city = city['city'], country = city['country'], url = city['url'], url_next = city['url_next'])
    reactor.stop()

crawl()
reactor.run() # the script will block here until the last crawl call is finished

