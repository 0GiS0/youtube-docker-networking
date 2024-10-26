# 5. Cómo funciona el networking en Docker

¡Hola developer 👋🏻! Este repo contiene las demos mostradas en el vídeo de mi canal de Youtube [5. Cómo funciona el networking en Docker](https://youtu.be/n5Zw00mYRH4).

[![3.Crea tus propias imágenes de Docker](docs/images/CAPÍTULO%205%20Gestionando%20Redes.png)](https://youtu.be/n5Zw00mYRH4)

Si no has visto el vídeo, te recomiendo que lo hagas para que entiendas el contexto de las demos que se muestran aquí.


## Port mapping


Si tenemos contenedores que están a la escucha a través de algún puerto, podemos mapear esos puertos a puertos de nuestro host para poder acceder a ellos. 

```bash
docker run -d -p 8080:80 nginx
```

En este caso es fácil adivinar que el contenedor de nginx está a la escucha en el puerto 80, por lo que mapeamos el puerto 8080 de nuestro host al puerto 80 del contenedor. Pero en el caso de que no sepamos en qué puerto está a la escucha el contenedor, podemos averiguarlo con el siguiente comando:

La instrucción `EXPOSE` nos ayuda a saber en qué puerto está a la escucha el contenedor, siempre y cuando la misma está bien documentada, ya que esta instrucción no tiene ningún efecto en el comportamiento del contenedor.

Podemos inspeccionar una imagen para saber en qué puerto está a la escucha el contenedor:

```bash
docker inspect nginx
```

En el vídeo te muestro otras formas de averiguarlo de una forma gráfica y más sencilla.

También puedes usar un parámetro como parte de la instrucción `docker run` para que Docker asigne un puerto aleatorio de tu host al puerto del contenedor:

```bash


## Network bridge



```bash
```


## Network host

```bash
```

## Network none

```bash
```

## Crea tus propias redes

```bash
```

## Limpiar recursos
    
```bash
 ```
