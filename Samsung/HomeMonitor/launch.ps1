$sdb = 'C:\tizen-studio\tools\sdb'

& $sdb install .buildResult/HomeMonitor.wgt
& $sdb shell "app_launcher -s M5aPw28OEp.HomeMonitor.Power"