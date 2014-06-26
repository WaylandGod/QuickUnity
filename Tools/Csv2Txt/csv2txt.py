# coding: utf-8

import os, sys

def csv2txt(source_path, output_path): 
	source_abspath = os.path.abspath(source_path) + "/"
	output_abspath = os.path.abspath(output_path) + "/"
	
	# Get all csv files in source path.
	csv_files = os.listdir(source_abspath)
	
	for f in csv_files:
		# Read conent of configuration file.
		print "Converting File ["+f+"]..."
		file_object = open(source_abspath + f, "r")
		txt_file = file_object.read()
		file_object.close()
		
		# Write content to new file with extensions txt.
		filename = os.path.splitext(f)
		new_path = output_abspath + filename[0] + ".txt"
		file_object = open(new_path, "w")
		file_object.write(txt_file.decode("GB2312").encode("UTF-8"))
		file_object.close()
	
	print "All done."
	
# csv configuration files relative path.
source_path = "../../Config"

# txt configuration files relative path.
output_path = "../../Assets/Examples/Resources/Config"

csv2txt(source_path, output_path)