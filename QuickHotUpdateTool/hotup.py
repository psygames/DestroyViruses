import itchat
import os
import time
from itchat.content import *

file_list = ["TableGameLevel.xlsx", "TableGameWave.xlsx", "TableVirus.xlsx", "TableFirePower.xlsx",
             "TableFireSpeed.xlsx", "TableBuff.xlsx", "TableAircraft.xlsx", ]

ready_tag = "ready"
finish_tag = "finish"


@itchat.msg_register([ATTACHMENT])
def download_files(msg):
    fn = msg.fileName
    if fn in file_list:
        res = msg.download(fn)
        open(ready_tag, "w").close()
        if res['BaseResponse']['Ret'] == 0:
            n = 3.0
            while n > 0:
                n = n - 0.1
                time.sleep(0.1)
                if os.path.exists(finish_tag):
                    break
            if os.path.exists(finish_tag):
                f = open(finish_tag)
                tx = f.read()
                f.close()
                os.remove(finish_tag)
                print("热更完成：\n" + tx)
                msg.user.send("热更完成：\n" + tx)
            else:
                print("热更失败，请联系管理员。")
                msg.user.send("热更失败，请联系管理员。")


itchat.auto_login(hotReload=True)
itchat.run()
