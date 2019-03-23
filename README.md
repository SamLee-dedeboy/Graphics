# Graphics
## 几何图形绘制系统使用说明
## 1）	启动程序：系统的可执行程序为Release文件夹中的myGraphics.exe，点击后会弹出系统程序，界面如下图
 ![avator](https://github.com/SamLee-dedeboy/picturesURL/blob/master/1.png)
## 2）	界面说明：左边的区域为绘图区，映射到一个平面直角坐标系。右边为已实现的功能，其中裁剪只能用于裁剪直线
## 3）	绘图说明：
### a)	所有的图形控制点的选择都是由鼠标单击完成，除了多边形的最后一个顶点使用双击。在绘制完图形后会自动选定该图形，并显示若干可选中的顶点，包括图形的控制点，中心点和旋转点。中心点显示为红色，旋转点在中心点上方，显示为黑色。此时，点击控制点可以修改图形的形状，点击中心点可以平移，点击旋转点可以进行旋转。再点击一次可以完成对图形的编辑，并且此时图形会再次被选中，可以继续修改。点击空白区域会取消选中。一旦取消选中就不可再选中。所有的操作都模仿主流的图形编辑交互系统。（此处有一个”feature”：在裁剪直线时，如果把直线整个裁剪掉，裁剪后会自动选中上一个绘制的图形。所以如果不小心误触了空白区域取消选中，可以再画一条直线，然后裁剪掉，就可以选中刚刚绘制的图形）
### b)	直线：两个控制点，在绘图区左键单击一个点即为起点，再单击另一个点为终点。
![avator](https://github.com/SamLee-dedeboy/picturesURL/blob/master/2.png)

### c)	矩形：绘图时有两个控制点，为所绘矩形的一条对角线的两端。绘制完后可以根据任意一个顶点进行编辑
 ![avator](https://github.com/SamLee-dedeboy/picturesURL/blob/master/3.png)
### d） 多边形：每次单击确定一个控制点，最后一个控制点双击确定。在鼠标移动的过程中会自动将鼠标当前的位置和第一个顶点进行连接，实现对绘制图形的预览。在多边形绘制完后，可以修改控制点的位置，改变多边形的形状。
修改控制点示例：
  ![avator](https://github.com/SamLee-dedeboy/picturesURL/blob/master/4.png)
  ![avator](https://github.com/SamLee-dedeboy/picturesURL/blob/master/5.png)
 
### e） 圆：两个控制点，第一个点确定的是圆心，第二个点确定半径大小。选中时显示四个控制顶点，可以修改半径大小。
### f） 椭圆：两个控制点，确定的是椭圆的外接矩形。选中时显示外接矩形的四个顶点，通过修改外接矩形的形状可以修改椭圆的形状。
### g） 贝塞尔曲线：绘制的操作实际上是确定若干控制点的操作，与绘制多边形的操作相似。绘制完后会显示控制点，修改控制点可以改变贝塞尔曲线的形状。但是不能加入和删除控制点。
### h）所有图形都可以平移和旋转。矩形，多边形，圆和椭圆可以填充。在选中时点击填充即可填充图形。Color按钮可以改变填充的颜色。裁剪只能裁剪直线。
### i） Clear：清空绘图板。Save：将绘图板上的图形保存为Release_image_x.bmp。x为保存的图片数目。保存目录与Release文件夹同目录。
### j） 显示OFF格式的三维模型：此功能使用matlab实现，代码实现写在off_loader.m中。使用matlab打开运行即可。
