#Copenhagen Ecology Urban Model
#Data Retrieval
#R 4.2.1
# 1 - Install necessary packages:
from logging import lastResort
import os as o
from pyexpat.errors import XML_ERROR_INCOMPLETE_PE
import geopandas as gpd
import pandas as pd
import numpy as np
import pathlib as pl
import shapely.geometry as sg 
from pyproj import crs
import overpy
import requests as rq
import ee
import datetime

# Trigger the authentication flow and initialize the library.
# ee.Authenticate() run once a week?
ee.Initialize()

# 2 - Define Functions:
def bbox_from_geopts(pt_a, pt_b):
    if (pt_a[0] < pt_b[0]):
        xmin = pt_a[0]
        xmax = pt_b[0]
    else:
        xmin = pt_b[0]
        xmax = pt_a[0]
    if (pt_a[1] < pt_b[1]):
        ymin = pt_a[1]
        ymax = pt_b[1]
    else:
        ymin = pt_b[1]
        ymax = pt_a[1]
    bb_poly = sg.Polygon([(xmin, ymin), (xmax, ymin), (xmax, ymax), (xmin, ymax)])
    return bb_poly

# 3 - Set Working Directory:
o.chdir("C:/Users/ATSmi/OneDrive/Documents/CITA/7A/GIS")
o.getcwd()

# 4 - Define Retrieval BoundingBox:
a_geopt: tuple = (55.666103, 12.549321)
b_geopt: tuple = (55.698544, 12.630742)
bbox_polygon: sg.Polygon = bbox_from_geopts(a_geopt, b_geopt)
bbox_df: pd.DataFrame = {'geometry': [bbox_polygon]}
bbox_gdf: gpd.GeoDataFrame = gpd.GeoDataFrame(bbox_df, crs="EPSG:4326")
bbox_bnd: pd.DataFrame = bbox_gdf.bounds

# 5 - Get OSM. Query Open Street Maps (OSM) via Overpass API:
headers = {
    'Connection': 'keep-alive',
    'sec-ch-ua': '"Google Chrome 80"',
    'Accept': '*/*',
    'Sec-Fetch-Dest': 'empty',
    'User-Agent': 'EcologyGIS_DataRetrievl / atsmithpb @ github',
    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
    'Origin': 'https://overpass-turbo.eu',
    'Sec-Fetch-Site': 'cross-site',
    'Sec-Fetch-Mode': 'cors',
    'Referer': 'https://overpass-turbo.eu/',
    'Accept-Language': '',
    'dnt': '1',
}
query = """nwr(55.666103,12.549321,55.695884,12.630742);out;"""
data = {'data': query}
response = rq.post('https://overpass-api.de/api/interpreter', headers=headers, data=data)
print(response)
with open('myquery.osm', 'w', encoding ='utf-8') as f:
    f.write(response.text)


# 6 - Get Google EE Data
ee_date = ee.Date('2022-10-12')
py_date = datetime.datetime.utcfromtimestamp(ee_date.getInfo()['value']/1000.0)
ee.Geometry.BBox(bbox_bnd.iat[0,0])


#api = overpy.Overpass()
#results = api.parse_xml(response.text)

#gdf_buildings = gpd.read_file("DATA/1084_SHAPE_UTM32-EUREF89/FOT/BYGNINGER/BYGNING.shp")
#print("Buildings .shp Loaded...")
#gdf_treeloc = gpd.read_file("DATA/1084_SHAPE_UTM32-EUREF89/FOT/NATUR/TRAE.shp")
#print("Tree Locations .shp Loaded...")
#gdf_treebasis = gpd.read_file("DATA/trae_basis/trae_basis.shp")
#print("Tree Basis .shp Loaded...")
#gdf_greenarea = gpd.read_file("DATA/park_groent_omr_oversigtskort/park_groent_omr_oversigtskort.shp")
#print("Green Areas .shp Loaded...")
#gdf_garden = gpd.read_file("DATA/storbyhaver/storbyhaver.shp")
#print("Gardens .shp Loaded...")
#gdf_gardenloc = gpd.read_file("DATA/1084_SHAPE_UTM32-EUREF89/FOT/NATUR/GARTNERI.shp")
#print("Garden Locations .shp Loaded...")
#gdf_uselimits = gpd.read_file("DATA/1084_SHAPE_UTM32-EUREF89/FOT/NATUR/BRUGSGRAENSE.shp")
#print("Use Limits .shp Loaded...")





