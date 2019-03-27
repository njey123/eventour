import re
from scrapy.exporters import JsonItemExporter
from termcolor import colored
class JsonPipeline(object):

    def __init__(self, dest):
        file = None
        print(colored("initialized", 'yellow'))
        self.dest = dest
        print(colored(self.dest,'red'))

    @classmethod
    def from_crawler(cls, crawler):
        return cls(dest = crawler.spider.dest)
    
    def open_spider(self, spider):
        self.file = open(self.dest+'.json', 'wb')
        self.exporter = JsonItemExporter(self.file)
        self.exporter.start_exporting()
        print(self.dest)
    
    def close_spider(self, spider):
        self.exporter.finish_exporting()
        print(colored("happened", 'blue'))
        self.file.close()

    def process_item(self, item, spider):
        # Process rating to 4.5 format
        item['rating']=str(float(re.findall('\d+', item['rating'])[0])/10)
        # Process review count to int format
        item['review_count']=str("".join(re.findall('\d+', item['review_count'])))
        # Extract duration and assign state
        if any("< 1 hour" in s for s in item['duration']):
            item['duration'] = '1'
        elif any("1-2 hours" in s for s in item['duration']):
            item['duration'] = '2'
        elif any("2-3 hours" in s for s in item['duration']):
            item['duration'] = '3'
        elif any("More than 3 hours" in s for s in item['duration']):
            item['duration'] = '4'
        else:
            item['duration'] = ''

        print(colored(self.file.name, 'red'))
        print(colored(self.dest, 'green'))
        self.exporter.export_item(item)
        return item