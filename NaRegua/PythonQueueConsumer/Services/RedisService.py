import redis

class RedisService:
    def __init__(self, host, port):
        self.host = host
        self.port = port
        self.connection = self.CreateConnection()

    def CreateConnection(self):
        return redis.Redis(host=self.host, port=self.port, db=0)

    def Ping(self):
        try:
            self.connection.ping()
            return True
        except redis.exceptions.ConnectionError:
            return False

    def Set(self, key, value):
        self.connection.set(key, value)

    def Get(self, key):
        value = self.connection.get(key)
        if value is not None:
            return value.decode('utf-8')
        return None
