 # 简介  
本项目是基于[我之前的光线追踪渲染器](https://github.com/pg7go/RayTracing) 实现的。  
算是我们计算机图形学课程的一个扩展实现了，在光追上加入一些偏向于实时渲染的功能，但是限于CPU性能还是达不到实时渲染的要求。  

# 功能  
- 只靠CPU的离线渲染  
- 内置两种渲染管线（迭代式光线追踪渲染管线、快速渲染管线）  
- 内置材质（Phong、Lit、Metal、Transparency、Dielectric、Lambertian）  
- 内置光线衰减器（叠加、相乘）  
- Phong光照模型  
- 硬阴影生成算法  
- 支持OBJ格式模型  
- 支持天空盒（内置线性渐变材质、HDRI全景贴图材质）  
- 支持多光源  
- 支持贴图（缩放、偏移）  
- 支持相机位移、朝向、聚焦  
- 渲染报告（实时渲染进度、时间、分辨率）  

# 渲染测试

![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/Render.png)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/new1.png)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/render2.gif)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/Chapter_12.png)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/Chapter_8.png)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/Chapter_9.png)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/Chapter_10.png)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/Chapter_11.png)  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/progress.png)  

# 环境
.Net Core 2.0   

# 性能
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/Screenshots/Render.png)   
三个苹果图片，525个面，分辨率1600×800，渲染了6分钟，CPU：i7-7700  
![img](https://raw.githubusercontent.com/pg7go/CPU-Render/master/RayTracing/bin/Debug/netcoreapp2.0/Render.png)   
一个苹果时，180个面，分辨率400×200，渲染了10秒，CPU：i7-7700  
  
# 使用方法
1.渲染一个场景，首先需要一个渲染器（宽度，高度，取样次数（抗锯齿））（这里使用光线追踪渲染管线RayTracingRenderPipeLine）  
`Render render = new Render(new RayTracingRenderPipeLine(400, 200, 100));`  
  
2.建立一个场景  
`Scene scene = new Scene();`  
  
3.添加一个可光线碰撞的物体  
`scene.hitableObjects.Add(new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new Vector3(0.8f, 0.3f, 0.3f))));`  
  
4.建立一个摄像机并设置位置和朝向  
`Camera camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -1), Vector3.UnitY, 90,2,0,1);`  
  
5.开始渲染  
`render.RenderScene(scene, camera)`  
  
6.保存文件并打开  
`ShowPic(render.SaveRenderedMap("Render.png"));`  



