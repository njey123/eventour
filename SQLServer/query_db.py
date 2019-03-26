import json
import pyodbc
import sys
import os


# =====================================
# Format of Data from Crawl - Not Used
# =====================================

class Attraction():
    def __init__(self, json_info):
        self.__dict__ = json.loads(json_info)


# =====================================
# Database Class
# =====================================

class EventourDB():
    def __init__(self, table_name, dest, cursor):
        self.table_name = table_name
        self.dest = dest
        self.cursor = cursor

    # Insert row in table
    def insert_row(self, attraction_name, rating, review_count, image_url, duration, description, address):
        print('\nInserting a new row into table...')
        tsql = "INSERT INTO " + self.table_name + " (dest, name, rating, review_count, image_url, duration, description, address) VALUES (?,?,?,?,?,?,?,?);"
        with self.cursor.execute(tsql, self.dest, attraction_name, rating, review_count, image_url, duration, description, address):
            print ('Successfully inserted!')

    # Delete row(s) from table
    def delete_row(self, attraction_name):
        print('\nDeleting the following attraction: ' + attraction_name + '...')
        tsql = "DELETE FROM " + self.table_name + " WHERE name = ?"
        with self.cursor.execute(tsql, attraction_name):
            print ('Successfully deleted!')

    # Query table in database and order results
    def query_table(self, cols_to_order):
        print('\nReading data from table...')
        attraction_db_results = []
        rating_db_results = []
        review_count_db_results = []
        image_url_db_results = []
        duration_db_results = []
        description_db_results = []
        address_db_results = []

        tsql = "SELECT dest, name, rating, review_count, image_url, duration, description, address FROM " + self.table_name + " WHERE dest LIKE '" + self.dest + "' ORDER BY " + cols_to_order + ";"
        with self.cursor.execute(tsql):
            row = self.cursor.fetchone()
            while row:
                attraction_db_results.append(row[1])
                rating_db_results.append(row[2])
                review_count_db_results.append(row[3])
                image_url_db_results.append(row[4])
                duration_db_results.append(row[5])
                description_db_results.append(row[6])
                address_db_results.append(row[7])

                print (str(row[0]) + "\t\t" + str(row[1]) + "\t\t" + str(row[2]) + "\t\t" + str(row[3]) + "\t\t" + str(row[4]) + "\t\t" + str(row[5]) + "\t\t" + str(row[6]) + "\t\t" + str(row[7]))
                row = self.cursor.fetchone()
        
        return attraction_db_results, rating_db_results, review_count_db_results, image_url_db_results, duration_db_results, description_db_results, address_db_results


# =====================================
# Main
# =====================================

def main(dest):
    # Database setup
    server = '0.0.0.0'
    database = 'EventourDB'
    username = 'sa'
    password = 'SeventyPies42'
    cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER='+server+';PORT=1443;DATABASE='+database+';UID='+username+';PWD='+ password)
    cursor = cnxn.cursor()

    # Name of table in database
    table_name = 'Attractions'
    eventourDB = EventourDB(table_name, dest, cursor)

    # -------------- FOR TESTING --------------
    # # Insert some data into database
    # dest = 'New York City, New York'
    # eventourDB = EventourDB(table_name, dest)
    # eventourDB.insert_row('Attraction 1', '4.0', '300', 'ImageURL1', '2')
    # eventourDB.insert_row('Attraction 2', '3.0', '500', 'ImageURL2', '3')
    # eventourDB.insert_row('Attraction 3', '5.0', '500', 'ImageURL3', '1')
    # eventourDB.insert_row('Attraction 4', '4.0', '1000', 'ImageURL1', '1')
    # eventourDB.insert_row('Attraction 5', '3.0', '700', 'ImageURL2', '3')
    # eventourDB.insert_row('Attraction 6', '5.0', '800', 'ImageURL3', '4')
     # -------------- FOR TESTING --------------

    # Query database
    cols_to_order = 'cast(review_count as int) DESC, cast(rating as float) DESC'
    attraction_db_results, rating_db_results, review_count_db_results, image_url_db_results, duration_db_results, description_db_results, address_db_results = eventourDB.query_table(cols_to_order)

    return attraction_db_results, rating_db_results, review_count_db_results, image_url_db_results, duration_db_results, description_db_results, address_db_results


