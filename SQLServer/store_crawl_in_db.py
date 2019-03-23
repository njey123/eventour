import pymssql


class DataPipeline():
    # Constructor
    def __init__(self, server, username, pwd, db, table_name, dest):
        self.conn = pymssql.connect(host=server, user=username, password=pwd, database=db)
        self.cursor = self.conn.cursor()
        self.table_name = table_name
        self.dest = dest
    
    # Insert row in table in database
    def process_item(self, item):
        print('\nInserting a new row into database...')
        try:
            tsql = "INSERT INTO " + self.table_name + "(dest, name, rating, review_count, image_url, duration) VALUES (%s, %s, %s, %s, %s, %s)" 
            self.cursor.execute(tsql, (self.dest, item['name'], item['rating'], item['review_count'], item['image_url'], item['duration']))
            self.conn.commit()
        except pymssql.Error as e:
            print("Error inserting row in database")

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


# =====================================
# Main
# =====================================

server = '0.0.0.0'
database = 'EventourDB'
username = 'sa'
password = 'SeventyPies42'

# Name of table in database
table_name = 'Attractions'
# Name of city to store information for in database
dest = 'New York City'

example_item =	{
  "name": "Name1",
  "rating": "Rating1",
  "review_count": "Count1",
  "image_url": "URL1",
  "duration": "Duration1"
}

# Insert some data into database and display it
data_pipeline = DataPipeline(server, username, password, database, table_name, dest)
data_pipeline.process_item(example_item)
data_pipeline.display_data()