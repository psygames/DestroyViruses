from django.shortcuts import render, HttpResponse
# from  app01 import  forms
# Create your views here.

import os

BASE_DIR = 'downloads/'
FILE_NAME_DIR = 'downloads/'

ready_tag = "../ready"
finish_tag = "../finish"


def upload(request):
    if request.method == 'POST':  # 获取对象
        obj = request.FILES.get('file_field')
        if not check_file_name(obj.name):
            return resp("不支持的热更文件：" + obj.name)
        save_file(obj)
        res = check_result()
        return resp(res)
    return render(request, 'index.html')


def save_file(obj):
    f = open(os.path.join(BASE_DIR, obj.name), 'wb')
    for chunk in obj.chunks():
        f.write(chunk)
    f.close()


def resp(_content):
    print(_content)
    return HttpResponse(_content)


def check_file_name(_name):
    return _name in os.listdir(FILE_NAME_DIR)


def check_result():
    import time
    n = 3.0
    while n > 0:
        n = n - 0.1
        time.sleep(0.1)
        if os.path.exists(finish_tag):
            break
    if os.path.exists(finish_tag):
        f = open(finish_tag, encoding="utf-8")
        tx = f.read()
        f.close()
        os.remove(finish_tag)
        return "热更完成：\n" + tx
    else:
        return "热更失败，请联系管理员。"
