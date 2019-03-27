from spider import TripAdvisorSpider
import scrapy
from scrapy.crawler import CrawlerProcess
import json
from twisted.internet import reactor, defer
from scrapy.crawler import CrawlerRunner
from scrapy.utils.log import configure_logging

configure_logging()
runner = CrawlerRunner({
    'USER_AGENT': 'Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)',
    'ITEM_PIPELINES' : {'pipeline.ItemPipeline':100}
    })

@defer.inlineCallbacks
def crawl():
    with open('cities.json') as f:
        cities = json.load(f)
    for city in cities:
        yield runner.crawl(TripAdvisorSpider,city = city['city'], country = city['country'])
    reactor.stop()

crawl()
reactor.run() # the script will block here until the last crawl call is finished