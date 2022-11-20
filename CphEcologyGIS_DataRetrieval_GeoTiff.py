#Copenhagen Ecology Urban Model
#Data Retrieval - GeoTiff
# 1 - Install necessary packages:
from logging import lastResort
import os as o
from pyexpat.errors import XML_ERROR_INCOMPLETE_PE
import geopandas as gpd
from matplotlib.hatch import NorthEastHatch
import pandas as pd
import numpy as np
import pathlib as pl
import shapely.geometry as sg 
from pyproj import crs
import overpy
import requests as rq
import ee
import datetime
import geemap
import pygbif as gbif

# Authenticate & Initialize Google Earth Engine
#ee.Authenticate() #run once a week?
ee.Initialize()
print('EE Initialized...')

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

def getEVI(ee_image):
    EVI = ee_image.expression(
        '2.5 * ((NIR - RED) / (NIR + 6 * RED - 7.5 * BLUE + 1))', {
            'NIR': ee_image.select('B8').divide(10000),
            'RED': ee_image.select('B4').divide(10000),
            'BLUE': ee_image.select('B2').divide(10000)
        }
    ).rename("EVI")
    ee_image = ee_image.addBands(EVI)
    return(ee_image)

# 3 - Set Working Directory:
o.chdir("C:/Users/ATSmi/OneDrive/Documents/CITA/7A/GIS")
o.getcwd()

# 4 - Define Data Retrieval BoundingBox:
a_geopt: tuple = (55.666103, 12.549321)
b_geopt: tuple = (55.698544, 12.630742)
bbox_polygon: sg.Polygon = bbox_from_geopts(a_geopt, b_geopt)
bbox_df: pd.DataFrame = {'geometry': [bbox_polygon]}
bbox_gdf: gpd.GeoDataFrame = gpd.GeoDataFrame(bbox_df, crs="EPSG:4326")
bbox_bnd: pd.DataFrame = bbox_gdf.bounds
north: float = bbox_bnd.iat[0,2]
south: float = bbox_bnd.iat[0,0]
east: float = bbox_bnd.iat[0,3]
west: float = bbox_bnd.iat[0,1]
print('BBox Initialized...')

# 5 - Get OSM. Query Open Street Maps (OSM) via Overpass API:
# headers = {
#     'Connection': 'keep-alive',
#     'sec-ch-ua': '"Google Chrome 80"',
#     'Accept': '*/*',
#     'Sec-Fetch-Dest': 'empty',
#     'User-Agent': 'EcologyGIS_DataRetrievl / atsmithpb @ github',
#     'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
#     'Origin': 'https://overpass-turbo.eu',
#     'Sec-Fetch-Site': 'cross-site',
#     'Sec-Fetch-Mode': 'cors',
#     'Referer': 'https://overpass-turbo.eu/',
#     'Accept-Language': '',
#     'dnt': '1',
# }
# query = f"""nwr({south},{west},{north},{east});out;"""
# data = {'data': query}
# print('Sending Query to Overpass...')
# response = rq.post('https://overpass-api.de/api/interpreter', headers=headers, data=data)
# print(f'Response: {response}')
# with open('myquery.osm', 'w', encoding ='utf-8') as f:
#     f.write(response.text)

# 6 - Retrieve Google EE Data within the BoundingBox as ee.Image objects
ee_date_start: ee.Date = ee.Date('2022-08-01')
ee_date_end: ee.Date = ee.Date('2022-08-30')
ee_bbox: ee.Geometry = ee.Geometry.BBox(west, south, east, north)
ee_bbox_poly: ee.Geometry = ee.Geometry.Polygon(ee_bbox._coordinates)
ee_bbox_rect: ee.Geometry = ee.Geometry.Rectangle(south, west, north, east)
ee_filter_s2:ee.Filter = ee.Filter.And(ee.Filter.bounds(ee_bbox),ee.Filter.date(ee_date_start, ee_date_end), ee.Filter.lt('CLOUDY_PIXEL_PERCENTAGE', 35)) 
ee_imgc_s2: ee.ImageCollection = ee.ImageCollection('COPERNICUS/S2_SR_HARMONIZED').filter(ee_filter_s2)\
    .select('B4', 'B3', 'B2', 'B8')
ee_img_s2: ee.Image = ee.Image(ee_imgc_s2.first())
ee_img_s2_index = ee_img_s2.get('system:index')
ee_filter_dw: ee.Filter = ee.Filter.eq('system:index', ee_img_s2_index)
ee_imgc_dw: ee.ImageCollection = ee.ImageCollection('GOOGLE/DYNAMICWORLD/V1').filter(ee_filter_dw)\
    .select('label')
ee_img_dw: ee.Image = ee.Image(ee_imgc_dw.first())
print('EE Data Recieved...')

# Calculate NDVI from S2 Red and NIR bands
ee_img_ndvi: ee.Image = ee_img_s2.normalizedDifference(['B8', 'B4'])
ee_img_evi: ee.Image = getEVI(ee_img_s2).select('EVI')

# Define color palettes
ee_palette_dw: list = ['419BDF', '397D49', '88B053', '7A87C6','E49635', 'DFC35A', 'C4281B', 'A59B8F','B39FE1']
ee_palette_ndvi: list = ['#d53e4f', '#f46d43', '#fdae61', '#f7f7f7','#f7f7f7', '#abdda4', '#3288bd', '#3288bd']
ee_palette_evi: list = ['#d73027', '#f46d43', '#fdae61', '#f7f7f7','#f7f7f7', '#a6d96a', '#66bd63', '#1a9850']

# Create RGB images based on visualization parameters
ee_img_dwviz: ee.Image = ee_img_dw.visualize(**{'bands': 'label','min': 0, 'max': 8, 'palette': ee_palette_dw})
ee_img_s2viz: ee.Image = ee_img_s2.visualize(**{'bands': ['B4', 'B3', 'B2'],'min': 0, 'max': 3000})
ee_img_ndviviz: ee.Image = ee_img_ndvi.visualize(**{'min': -1, 'max': 1, 'palette': ee_palette_ndvi})
ee_img_eviviz: ee.Image = ee_img_evi.visualize(**{'bands': 'EVI','min': -1, 'max': 1, 'palette': ee_palette_evi})

print('EE Data Ready...')
ee_task_dw = ee.batch.Export.image.toDrive(image = ee_img_dwviz,  # an ee.Image object.
                                     region = ee_bbox_poly,  # an ee.Geometry object.
                                     description = 'EarthEngine_DyanmicWorld_CRS25832',
                                     folder = 'Data',
                                     fileNamePrefix = 'EE_DW_25832',
                                     scale = 10,
                                     crs = 'EPSG:25832')

ee_task_s2 = ee.batch.Export.image.toDrive(image = ee_img_s2viz,  # an ee.Image object.
                                     region = ee_bbox_poly,  # an ee.Geometry object.
                                     description = 'EarthEngine_Sentinal2_SurfaceReflect_CRS25832',
                                     folder = 'Data',
                                     fileNamePrefix = 'EE_S2SR_25832',
                                     scale = 10,
                                     crs = 'EPSG:25832')

ee_task_ndvi = ee.batch.Export.image.toDrive(image = ee_img_ndviviz,  # an ee.Image object.
                                     region = ee_bbox_poly,  # an ee.Geometry object.
                                     description = 'EarthEngine_Normalized-Difference-Vegitation-Index_CRS25832',
                                     folder = 'Data',
                                     fileNamePrefix = 'EE_NDVI_25832',
                                     scale = 10,
                                     crs = 'EPSG:25832')

ee_task_evi = ee.batch.Export.image.toDrive(image = ee_img_eviviz,  # an ee.Image object.
                                     region = ee_bbox_poly,  # an ee.Geometry object.
                                     description = 'EarthEngine_Enhanced-Vegitation-Index_CRS25832',
                                     folder = 'Data',
                                     fileNamePrefix = 'EE_EVI_25832',
                                     scale = 10,
                                     crs = 'EPSG:25832')
                                     
#ee_task_evi.start()
#ee_task_ndvi.start()
#ee_task_dw.start()
#ee_task_s2.start()
print('Images Exported Successfully...')

#api = overpy.Overpass()
#results = api.parse_xml(response.text)

gbif_bbox_poly: str = f'POLYGON(({west} {south}, {east} {south}, {east} {north}, {west} {north}, {west} {south}))'
gbif_fields = [
                            'key',
                            'kingdom',
                            'genus',
                            'species',
                            'genericName',
                            'iuncRedListCategory',
                            'geodeticDatum',
                            'decimalLongitude',
                            'decimalLatitude',
                            'coordinateUncertaintyInMeters',
                            'eventDate',
                            'habitat']
print('Downloading GBIF Data...')
gbif_dict_occ: dict = gbif.occurrences.search(geometry=gbif_bbox_poly, limit=10)
gbif_df_occ: pd.DataFrame = pd.DataFrame.from_dict(gbif_dict_occ['results'])
gbif_df_occ_filt: pd.DataFrame = gbif_df_occ.filter(items=gbif_fields)
gbif_gdf_wkt = "POINT (" + gbif_df_occ_filt.decimalLatitude.map(str) + " " + gbif_df_occ_filt.decimalLongitude.map(str) + ")"
gbif_df_occ_filt.rename(columns={'coordinateUncertaintyInMeters':'coordUIM'}, inplace=True)
gbif_gs = gpd.GeoSeries.from_wkt(gbif_gdf_wkt)
gbif_gdf: gpd.GeoDataFrame = gpd.GeoDataFrame(gbif_df_occ_filt, geometry=gbif_gs, crs="EPSG:25832")

print('Saving GBIF Shapefile...')
#gbif_df_occ_filt.to_csv(path_or_buf = 'C:/Users/ATSmi/Desktop/Test.csv', encoding = 'utf-8')
gbif_gdf.to_file('C:/Users/ATSmi/Desktop/GBIF_25832.shp')  