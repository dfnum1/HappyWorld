#coding=utf-8
import sys
import subprocess
import os

PROTO_DIR="./proto/"
PROTO_CPP_DIR="./cpp/"
PROTO_JAVA_DIR="../../Server/work_spaces/Game-Message/src/main/java/"
PROTO_CS_DIR="../../Client/Assets/Scripts/MainScripts/NetWork/Message/"
PROTO_SVR_CS_DIR="../../Server/work_spaces/CommonLibs/Source/Protos/"

PROTO_IMPORT_PATH=PROTO_DIR

PROTO_EXE_PATH="./protoc.exe"
PROTOGEN_EXE_PATH="./protocgen3.exe"

def log( str ):
    print( str )
	
def get_all_path(open_file_path):
    rootdir = open_file_path
    path_list = []
    list = os.listdir(rootdir)  # 列出文件夹下所有的目录与文件
    for i in range(0, len(list)):
        com_path = os.path.join(rootdir, list[i])
        #print(com_path)
        if os.path.isfile(com_path):
            path_list.append(com_path)
        if os.path.isdir(com_path):
            path_list.extend(get_all_path(com_path))
    #print(path_list)
    return path_list

def MakeCpp( file_name ):
    dst_dir = "--cpp_out=%s" % ( PROTO_CPP_DIR, )
    target_proto = file_name
    cmd = [ PROTO_EXE_PATH, dst_dir, target_proto ]
    #subprocess.call( cmd, shell=False, stdout = z, stderr = subprocess.PIPE )
    sp = subprocess.Popen( cmd, shell=False, stderr=subprocess.PIPE )
    errs = sp.stderr.readlines()
    sp.communicate()
    #print(errs)
    if ( len(errs) > 0 ):
        for err in errs:
            err_out = err.decode("utf-8")
            err_out = err_out.replace("\r\n", "")
            #log( err_out )
            log( "[ERROR] " + err_out )
    else:
        log("[SUCC] Make Cpp %s.pb.h And %s.pb.cpp." % (file_name, file_name))	

def MakeCs( file_name ):
    dst_dir = "--csharp_out=%s" % ( PROTO_CS_DIR, )
    target_proto = file_name
    cmd = [ PROTOGEN_EXE_PATH, dst_dir, target_proto ]
    #subprocess.call( cmd, shell=False )
    sp = subprocess.Popen( cmd, shell=False, stdout=None, stderr=subprocess.PIPE  )
    
    #log("Make Cs %s.cs." % (file_name,))
    errs = sp.stderr.readlines()
    sp.communicate()
    #print(errs)
    
    if ( len(errs) > 0 ):
        for err in errs:
            err_out = err.decode("utf-8")
            err_out = err_out.replace("\r\n", "")
            #log( err_out )
            log( "[ERROR] " + err_out )
    else:
        log("[SUCC] Make Cs %s.cs." % (file_name,))  
        
def MakeServerCs( file_name ):
    dst_dir = "--csharp_out=%s" % ( PROTO_SVR_CS_DIR, )
    target_proto = file_name
    cmd = [ PROTOGEN_EXE_PATH, dst_dir, target_proto ]
    #subprocess.call( cmd, shell=False )
    sp = subprocess.Popen( cmd, shell=False, stdout=None, stderr=subprocess.PIPE  )
    
    #log("Make Cs %s.cs." % (file_name,))
    errs = sp.stderr.readlines()
    sp.communicate()
    #print(errs)
    
    if ( len(errs) > 0 ):
        for err in errs:
            err_out = err.decode("utf-8")
            err_out = err_out.replace("\r\n", "")
            #log( err_out )
            log( "[ERROR] " + err_out )
    else:
        log("[SUCC] Make Cs %s.cs." % (file_name,))          

def MakeJava( file_name ):
    dst_dir = "--java_out=%s" % ( PROTO_JAVA_DIR )
    target_proto = file_name
    cmd = [ PROTO_EXE_PATH, dst_dir, target_proto ]
    #subprocess.call( cmd, shell=False )
    sp = subprocess.Popen( cmd, shell=False, stdout=None, stderr=subprocess.PIPE  )
    
    #log("Make Cs %s.cs." % (file_name,))
    errs = sp.stderr.readlines()
    sp.communicate()
    #print(errs)
    
    if ( len(errs) > 0 ):
        for err in errs:
            err_out = err.decode("utf-8")
            err_out = err_out.replace("\r\n", "")
            #log( err_out )
            log( "[ERROR] " + err_out )
    else:
        log("[SUCC] Make Java %s.java" % (file_name,))

def Make( file_name, mode ):
    #log("Make %s" % (file_name,))
    if mode == "cpp":
        MakeCpp( file_name )
    elif mode == "cs":
        MakeCs( file_name )
        pass
    else:
        log("Error Mode")

def MakeAll(mode):
	# TO DO
	log("Make %s" % (mode,))
	path_list = []
	path_list = get_all_path(PROTO_DIR)	
	if mode == "java":
		for x in path_list:
			MakeJava(x)
	elif mode == "cs":
		igore_dir = []#["proto/server"]
		cando = 1
		for x in path_list:
			cando = 1
			for dir in igore_dir:
				if dir in x:
					cando = 0
			if cando == 1:
				MakeCs(x)	
	elif mode == "svrcs":
		igore_dir = []#["proto/server"]
		cando = 1
		for x in path_list:
			cando = 1
			for dir in igore_dir:
				if dir in x:
					cando = 0
			if cando == 1:
				MakeServerCs(x)	                
	elif mode == "cpp":
		for x in path_list:
			MakeCpp(x)		
	else:
		log("Error Mode")
	
if __name__ == "__main__":
	argv = sys.argv
	argc = len(argv)
	
	file_name = ""
	mode = "both"
	
	if argc > 1:
		mode = argv[1].strip()
	if argc > 2:
		file_name = argv[2].strip()
		
	if len(file_name)>0:
		Make(file_name, mode)
	else:
		MakeAll(mode)
	