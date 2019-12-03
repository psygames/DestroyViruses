from django.shortcuts import render, HttpResponse
# from  app01 import  forms
# Create your views here.

import os

BASE_DIR = 'downloads/'

ready_tag = "../ready"
finish_tag = "../finish"


def upload(request):
    if request.method == 'POST':  # 获取对象
        obj = request.FILES.get('file_field')
        # 上传文件的文件名
        print("upload -> " + obj.name)
        f = open(os.path.join(BASE_DIR, obj.name), 'wb')
        for chunk in obj.chunks():
            f.write(chunk)
        f.close()
        res = check_result()
        return HttpResponse(res)
    return render(request, 'index.html')


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
        print("热更完成：\n" + tx)
        return "热更完成：\n" + tx
    else:
        print("热更失败，请联系管理员。")
        return "热更失败，请联系管理员。"
