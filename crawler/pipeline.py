import re
from scrapy.exporters import JsonItemExporter
import pymssql


class ItemPipeline(object):

    #file = None
    # Constructor
    def __init__(self, dest):
        self.conn = pymssql.connect(host='0.0.0.0', user='sa', password='SeventyPies42', database='EventourDB')
        self.cursor = self.conn.cursor()
        self.table_name = 'Attractions'
        self.dest = dest
    
    file = None
    @classmethod
    def from_crawler(cls, crawler):
        return cls(dest = crawler.spider.dest)

    def process_item(self, item, spider):
        # Process rating to 4.5 format
        item['rating']=str(float(re.findall('\d+', item['rating'])[0])/10)
        # Process review count to int format
        item['review_count']=str("".join(re.findall('\d+', item['review_count'])))
        # Extract duration and assign state
        if any("< 1 hour" in s for s in item['duration']):
            item['duration'] = '0'
        elif any("1-2 hours" in s for s in item['duration']):
            item['duration'] = '1'
        elif any("2-3 hours" in s for s in item['duration']):
            item['duration'] = '2'
        elif any("More than 3 hours" in s for s in item['duration']):
            item['duration'] = '3'
        else:
            item['duration'] = ''
        
        print('\nInserting a new row into database...')
        try:
            tsql = "INSERT INTO " + self.table_name + "(dest, name, rating, review_count, image_url, duration) VALUES (%s, %s, %s, %s, %s, %s)" 
            self.cursor.execute(tsql, (self.dest, item['name'], item['rating'], item['review_count'], item['image_url'], item['duration'], item['description'], item['address']))
            self.conn.commit()
        except pymssql.Error as e:
            print("Error inserting row in database")
        
        #self.exporter.export_item(item)
        return item
    
    # Display data in database for one destination (city)
    def display_data(self):
        print('\nReading data from database...')
        tsql = "SELECT dest, name, rating, review_count, image_url, duration FROM " + self.table_name + " WHERE dest LIKE '" + self.dest + "';"
        self.cursor.execute(tsql)  
        row = self.cursor.fetchone()  
        while row:  
            print (str(row[0]) + "\t\t" + str(row[1]) + "\t\t" + str(row[2]) + "\t\t" + str(row[3]) + "\t\t" + str(row[4]) + "\t\t" + str(row[5]))    
            row = self.cursor.fetchone()
