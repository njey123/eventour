# Import flask microframework library
from flask import Flask, request

from datetime import date
import json
import sys
import os

# Custom packages
sys.path.insert(0, os.getcwd() + '/SQLServer')
import query_db
sys.path.insert(0, os.getcwd() + '/utilities')
import trip_builder


# Initialize the flask application
app = Flask(__name__)

# Default Flask local web server: http://127.0.0.1:5000/

# Endpoint with post method
@app.route("/api/v1.0/csharp_python_restfulapi_json", methods=["POST"])
def csharp_python_restfulapi_json():
    """
    simple c# test to call python restful api web service
    """
    try:                
#         get request json object
        request_json = request.get_json()      
#         convert to response json object 
        response = jsonify(request_json)
        response.status_code = 200  
    except:
        exception_message = sys.exc_info()[1]
        response = json.dumps({"content":exception_message})
        response.status_code = 400
    return response

@app.route("/test", methods=["GET", "POST"])
def test_route():
    # Get data from client
    dest = request.form.get('dest')
    start_date = request.form.get('start_date')
    end_date = request.form.get('end_date')

    # Print to console what Xamarin client sent
    print("XAMARIN SENT US TO {}, FROM {} TO {}".format(dest, start_date, end_date))

    # Calulate length of trip in days
    # Date format is dd/mm/yyyy
    first_date = start_date.split('/')
    last_date = end_date.split('/')
    d0 = date(int(first_date[2]), int(first_date[1]), int(first_date[0]))
    d1 = date(int(last_date[2]), int(last_date[1]), int(last_date[0]))
    delta = d1 - d0
    num_days = delta.days + 1

    # Retrieve data from database
    attraction_db_results, rating_db_results, review_count_db_results, image_url_db_results, duration_db_results = query_db.main(dest)

    # Create a trip with the highest review count and ratings
    attractions, ratings, review_counts, image_urls, durations = trip_builder.suggest_trip(num_days, attraction_db_results, rating_db_results, review_count_db_results, image_url_db_results, duration_db_results)

    # Send response from server to client
    response_json = {
        'Dest': dest,
        'StartDate': start_date,
        'EndDate': end_date,
        'Attractions': attractions,
        'Ratings': ratings,
        'ReviewCounts': review_counts,
        'ImageURLs': image_urls,
        'Durations': durations
    }
    return json.dumps(response_json)

if __name__ == "__main__":
#     Run flask application in debug mode
    app.run(debug=True)  # Make sure to change debug=True to False in a production environment
