from scrapy.item import Item, Field

class AttractionItem(Item):
    name = Field()
    rating = Field()
    review_count = Field()
    img_url = Field()
    duration = Field()

