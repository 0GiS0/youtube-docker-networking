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
docker run -d --publish-all nginx
```

o bien, si quieres utilizar un comando más corto:


```bash
docker run -d -P nginx
```

Al igual que cualquier servidor, un contenedor puede estar a la escucha en varios puertos, solo tendrías que hacer que tu código esté a la escucha en varios puertos y en el Dockerfile puedes especificar los puertos que quieres exponer.

```Dockerfile
FROM nginx

EXPOSE 80/tcp
EXPOSE 80/udp

EXPOSE 8080
EXPOSE 8081
EXPOSE 8082
```

Este lo tienes en el archivo `Dockerfile.port-mapping` de este repositorio.

Y de la misma forma que lo hicimos antes, podríamos usar el comando `docker run -P` para que Docker asigne puertos aleatorios de nuestro host a los puertos del contenedor.


## Cómo podemos hacer que los contenedores se comuniquen entre sí

Todos los contenedores que hemos creado hasta ahora siempre han pertenecido a una red, sin tu saberlo. Docker crea una red por defecto para que los contenedores puedan comunicarse entre sí. 

Si echamos un vistazo a las redes que tenemos en nuestro sistema, podemos ver que Docker ha creado una red llamada `bridge`:

```bash
docker network ls
```

Esta es la red por defecto que Docker crea para que los contenedores puedan comunicarse entre sí.

### Network bridge

Si queremos ver el detalle de la misma, podemos hacerlo con el siguiente comando:

```bash
docker network inspect bridge
```

El resultado tiene un apartado llamado `Containers` que nos muestra los contenedores que pertenecen a esta red. Cada uno de ellos tiene su propia dirección IP. Te puedes dar cuenta además de que los rangos de esta red no son el mismo que el de tu host, esto es porque Docker crea una red virtual para que los contenedores puedan comunicarse entre sí. Si quieres que los contenedores se comuniquen entre sí, puedes hacerlo a través de la dirección IP de cada contenedor.

Puedes utilizar el contenedor `busybox` para hacer ping a otro contenedor. 

```bash
docker run -it --rm busybox
```

Una vez dentro del contenedor, puedes hacer una llamada a otro contenedor. Por ejemplo, si el otro contenedor es un servidor web y está a la escucha en el puerto 80, puedes hacer un `wget` a la dirección IP del contenedor:

```bash
wget -qO-  http://172.17.0.2
```

### Network host

Esta red nos permite que el contenedor comparta la red del host. Esto significa que el contenedor no tendrá su propia dirección IP, sino que compartirá la misma que el host. 

```bash
docker run -d --network host nginx
```

En este caso el contenedor de nginx no tiene su propia dirección IP, sino que comparte la misma que el host. Por lo que si abro un navegador y pongo `http://localhost`, veré el contenido del contenedor de nginx.

En el pasado esto no era posible probarlo con Docker Desktop pero con las versiones más recientes del mismo ya es posible como puedes ver en el vídeo.

### Network none

Este tipo de red no tiene acceso a la red del host ni a la red de otros contenedores. 

```bash
docker run -it --rm --network none busybox
```

Si ahora lanzo un `ifconfig` dentro del contenedor, verás que no tiene ninguna interfaz de red.

```bash
ifconfig
```

Solo tiene la interfaz `lo` que es la interfaz de loopback.

### Crea tus propias redes

Cuando instalamos Docker viene de caja un servidor DNS pero solo funciona en las redes que creamos nosotros. Y para crear nuestras propias redes, podemos hacerlo con el siguiente comando:

```bash
docker network create returngis
```

A partir de este momento tenemos una nueva red para nuestros contenedores. He creado un nuevo Dockerfile llamado `Dockerfile.network-test`para las pruebas con esta red. Puedes generar la imagen utilizando este comando:

```bash
docker build -t ubuntu-with-net-tools -f Dockerfile.network-test .
```

Y ahora lo siguiente que vamos a hacer es crearnos dos contenedores que formen parte de la nueva red:

```bash
docker run --name donpepito --network returngis -it ubuntu-with-net-tools
```

Ahora haz un split del terminal para crear el segundo contenedor:

```bash
docker run --name donjose --network returngis -it ubuntu-with-net-tools
```

Y a partir de este momento puedes hacer ping de un contenedor a otro:

```bash
ping donjose
```

o desde el otro contenedor:

```bash
ping donpepito
```

También podrías hacer un `ping` directamente a la dirección IP del contenedor:

```bash
ping 172.18.0.2
```

O desde el otro contenedor:

```bash
ping 172.18.0.3
```

## Cómo ejecutar la aplicación de ejemplo Tour Of Heroes

Durante el vídeo te explico todos los cambios que he realizado en la API de Tour Of Heroes para que pueda comunicarse con la base de datos. El código de la misma esta en el directorio `tour-of-heroes-api`.

Ahora para ejecutar la misma lo primero que necesitas es crear una red:

```bash
docker network create tour-of-heroes-vnet
```

Creamos la base de datos dentro de la red:

```bash
docker run --name db -e ACCEPT_EULA=Y -e SA_PASSWORD=Password123 -d -p 1433:1433 --network tour-of-heroes-vnet -d mcr.microsoft.com/azure-sql-edge
```

Ahora generamos la imagen de la API:

```bash
cd tour-of-heroes-api
docker build -t tour-of-heroes-api:v1 .
```

Y una vez que finalice la misma, ejecutamos el contenedor:

```bash
docker run --name api --network tour-of-heroes-vnet -p 5051:5000 -d  tour-of-heroes-api:v1
```

En este caso la API si que está expuesto porque el frontal web necesita comunicarse con la misma.

Si quieres probar el frontal web en Angular necesitas crear la imagen de la misma:

```bash
cd tour-of-heroes-angular
docker build -t tour-of-heroes-web:v1 -f Dockerfile.gh-copilot .
```

Y y a para finalizar ejecutamos el contenedor:

```bash
docker run --name web --network tour-of-heroes-vnet -p 80:80 -d tour-of-heroes-web:v1
```

Para entender mejor todos estos conceptos, te recomiendo que veas el vídeo de mi canal de Youtube [5. Cómo funciona el networking en Docker](https://youtu.be/n5Zw00mYRH4).

¡Nos vemos 👋🏻!