FROM microsoft/dotnet:1.1.0-sdk-projectjson

RUN apt-get update && apt-get upgrade -y
RUN apt-get install -y sqlite3 libsqlite3-dev --fix-missing
RUN wget -qO- https://deb.nodesource.com/setup_4.x | bash -
RUN apt-get install -y build-essential nodejs

COPY . /app

WORKDIR /app

RUN ["npm", "install", "-g", "bower"]
RUN ["npm", "install", "-g", "gulp"]
RUN echo '{ "allow_root": true }' > /root/.bowerrc
RUN ["dotnet", "restore"]
RUN ["dotnet", "publish"]
RUN ["dotnet", "build"]
RUN ["dotnet", "ef", "database", "update"]
RUN ["sh", "init.sh"]

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]