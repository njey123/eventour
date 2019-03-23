import scrapy
from search import getCityUrl

from items import AttractionItem

class TripAdvisorSpider(scrapy.Spider):
    name = 'trip_advisor_spider'
    allowed_domains = ['www.tripadvisor.ca/']
    urls = getCityUrl('New York') #get url for the city attraction search result page and the next page
    start_urls = [urls[0]] #use the first url as start url
    
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
        '''
        item = AttractionItem()
        item['name'] = response.css(NAME_SELECTOR).extract_first()
        item['rating'] = response.css(RATING_SELECTOR).xpath('span/@class').extract_first()
        item['review_count'] = response.css(REVIEW_COUNT_SELECTOR).extract_first()
        item['image_url'] = response.css(IMAGE_SELECTOR).xpath("@src").extract_first()
        item['duration'] = response.css(DURATION_SELECTOR).extract()
        yield item
        '''
        yield {
            'name': response.css(NAME_SELECTOR).extract_first(),
            'rating': response.css(RATING_SELECTOR).xpath('span/@class').extract_first(),
            'review_count': response.css(REVIEW_COUNT_SELECTOR).extract_first(),
            'image_url': response.css(IMAGE_SELECTOR).xpath("@src").extract_first(),
            'duration': response.css(DURATION_SELECTOR).extract()
        }