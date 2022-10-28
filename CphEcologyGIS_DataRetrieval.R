#Copenhagen Ecology Urban Model
#Data Retrieval
#R 4.2.1
# 1 - Install necessary packages:
if ("osmar" %in% rownames(installed.packages()) == FALSE)
{
  packageurl <- "https://cran.r-project.org/src/contrib/Archive/osmar/osmar_1.0-01.tar.gz"
  install.packages(packageurl, repos = NULL, type = "source")
  library(sp)
} else {
  library(sp)
}

if ("magrittr" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("magrittr")
  library(sp)
} else {
  library(sp)
}

if ("dplyr" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("dplyr")
  library(sp)
} else {
  library(sp)
}

if ("sp" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("sp")
  library(sp)
} else {
  library(sp)
}

if ("sf" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("sf")
  library(sf)
  sf::sf_use_s2(FALSE) # command avoids pkg. "sf" (Simple Features) to use parent pkg. "sp" (Spatial data) spherical geometries
} else {
  library(sf)
  sf::sf_use_s2(FALSE)
}

if ("terra" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("terra")
  library(terra)
} else {
  library(terra)
}

if ("Rcpp" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("Rccp")
  library(Rcpp)
} else {
  library(Rcpp)
}

if ("osmdata" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("osmdata")
  library(osmdata)
} else {
  library(osmdata)
}

if ("tmaptools" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("tmaptools")
  library(tmaptools)
} else {
  library(tmaptools)
}

if ("leaflet" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("leaflet")
  library(leaflet)
} else {
  library(leaflet)
}

if ("tidygraph" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("tidygraph")
  library(tidygraph)
} else {
  library(tidygraph)
}

if ("plyr" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("plyr")
  library(plyr)
} else {
  library(plyr)
}

if ("dplyr" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("dplyr")
  library(dplyr)
} else {
  library(dplyr)
}

if ("reshape2" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("reshape2")
  library(reshape2)
} else {
  library(reshape2)
}

if ("jsonify" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("jsonify")
  library(jsonify)
} else {
  library(jsonify)
}

if ("dbscan" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("dbscan")
  library(dbscan)
} else {
  library(dbscan)
}

if ("purrr" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("purrr")
  library(purrr)
} else {
  library(purrr)
}

if ("RColorBrewer" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("RColorBrewer")
  library(RColorBrewer)
} else {
  library(RColorBrewer)
}

if ("nomisr" %in% rownames(installed.packages()) == FALSE)
{
  install.packages("nomisr")
  library(nomisr)
} else {
  library(nomisr)
}


# 2 - Set working directory
setwd("C:/Users/ATSmi/OneDrive/Documents/CITA/7A/GIS") # Set working directory...


# 3 - Setting bounding box area of analysis:
# Central Copenhagen Analysis Bounding Box and Analysis Polygon
CPH_longitude <- 12.5763
CPH_latitude <- 55.67780
Analysis_radius <- 10000
CPH_center <- c(CPH_longitude,CPH_latitude) # Set Analysis Center point...
Analysis_area <- c(Analysis_radius,Analysis_radius) # Set Analysis area (rectangle)...
Analysis_BBox <- osmar::bbox2coords(CPH_center, Analysis_area) # Set Analysis Bounding Box...
BBox_poly <- sf::st_as_sf(tmaptools::bb_poly(Analysis_BBox)) # Convert Bounding Box to sf polygon...

# Plotting BBox_poly on leaflet
leaflet::leaflet() %>%
  leaflet::addTile %>%
  leaflet::addPolygons(data = BBox_poly) #ignore warning (default map is OSM)


# 4a - Download data from O.S.M Overpass API (Comment out After Download)
#OSM_query <- Analysis_BBox %>%
#  osmdata::opq() # generate query for overpass API (Open Street Maps)
#osmdata::osmdata_xml(OSM_query, filename = "osm_df.osm") # return query in in .osm format

# 4b - Load data from .osm (Comment out After Download)
#OSM_df <- osmdata::osmdata_sf(doc = "osm_df.osm", stringsAsFactors = FALSE) # Load data...

# 4c - Load Data From Other Shape Files
OS_Buildings <- sf::read_sf("DATA/1084_SHAPE_UTM32-EUREF89/FOT/BYGNINGER/BYGNING.shp")
OS_Trees <- sf::read_sf("DATA/1084_SHAPE_UTM32-EUREF89/FOT/NATUR/TRAE.shp")
OS_TreeData <- sf::read_sf("DATA/trae_ basis/trae_basis.shp")
OS_GreenSpaces <- sf::read_sf("DATA/park_groent_omr_oversigtskort/park_groent_omr_oversigtskort.shp")
OS_MunicipalGardens <- sf::read_sf("DATA/storbyhaver/storbyhaver.shp")
OS_Gardens <- sf::read_sf("DATA/1084_SHAPE_UTM32-EUREF89/FOT/NATUR/GARTNERI.shp")
OS_LandUse <- sf::read_sf("DATA/1084_SHAPE_UTM32-EUREF89/FOT/NATUR/BRUGSGRAENSE.shp")

# 4d - Load LIDAR data
# Load Digital Surface Model (DSM)
DSM_fileList <- list.files(path = "DATA/DSM_617_72_TIF_UTM32-ETRS89", full.names = TRUE) # List of .tiff file names
DSM_length <- length(DSM_fileList) # Number of .tiff files in list
DSM_rastVec <- vector(mode = "list", DSM_length) # Create empty list for [rast] objects
for (i in 1 : DSM_length) { # Populate list with new [rast] objects
  DSM_temp <- terra::rast(DSM_fileList[i])
  DSM_rastVec[[i]] <- DSM_temp
}
DSM_crs <- sf::st_crs(DSM_temp)

# Load Digital Terrain Model (DTM)
DTM_fileList <- list.files(path = "DATA/DTM_617_72_TIF_UTM32-ETRS89", full.names = TRUE) # List of .tiff file names
DTM_length <- length(DTM_fileList) # Number of .tiff files in list
DTM_rastVec <- vector(mode = "list", DTM_length) # Create empty list for [rast] objects
for (i in 1 : DTM_length) { # Populate list with new [rast] objects
  DTM_temp <- terra::rast(DTM_fileList[i])
  DTM_rastVec[[i]] <- DTM_temp
}
DTM_crs <- sf::st_crs(DTM_temp)

print(DTM_temp)

# 5 - Crop Data
BBox_poly_25832 <- sf::st_transform(BBox_poly, 25832) # Transform BBox to Lidar crs
BBox_poly_9001 <- sf::st_transform(BBox_poly, 9001) # Transform BBox to DSM crs

# DSM <- raster::crop(DSM, raster::extent(sf::as_Spatial(sf::st_transform(sf::st_as_sf(BBox_poly), 27700)))) # cropping the raster with our bounding box, bear in mind that we are re-projecting the bbox to epsg:27700 crs and migrating the data from sf to sp system as the sp package is the base of the raster package
DSM_Crop <- terra::crop


# Digital Terrain Model (DTM)
DTM <- raster::raster("Lidar_National_Program/National-LIDAR-Programme-DTM-2020-TQ38sw/P_12151/DTM_TQ3080_P_12151_20201212_20201212.tif") # loading raster file DIGITAL TERRAIN MODEL (DTM)
DTM <- raster::crop(DTM, raster::extent(sf::as_Spatial(sf::st_transform(sf::st_as_sf(BBox_poly), 27700)))) # Cropping raster

# Plotting the raster - please uncomment and run to see plots

#slope_DSM <- raster::terrain(DSM, opt = 'slope') # slope map
#aspect_DSM <- raster::terrain(DSM, opt = 'aspect') # height aspect
#hill_DSM <- raster::hillShade(slope_DSM, aspect_DSM, 40, 270) # hill shade
#raster::plot(hill_DSM, col = grey(0:100/100), legend = FALSE, main = 'Analysis Area') # plot 1
#raster::plot(DSM, col = rainbow(25, alpha = 0.35), add = TRUE) # plot 2
#slope_DTM <- raster::terrain(DTM, opt = 'slope') # slope map
#aspect_DTM <- raster::terrain(DTM, opt = 'aspect') # height aspect
#hill_DTM <- raster::hillShade(slope_DTM, aspect_DTM, 40, 270) # hill shade
#raster::plot(hill_DTM, col = grey(0:100/100), legend = FALSE, main = 'Analysis Area') # plot 3
#raster::plot(DTM, col = rainbow(25, alpha = 0.35), add = TRUE) # plot 4


BBox_poly <- sf::`st_crs<-`(BBox_poly, 9001) # Assign CRS (Coordinate Reference System) to BBox...
sf::st_crs(BBox_poly) # Copenhagen uses EPSG: 9001 - Mercator uses EPSG: 4326)


#6 - Saving the Data
Save_folder <- "C://Users//ATSmi//OneDrive//Documents//CITA//7A//GIS//SAVE"

