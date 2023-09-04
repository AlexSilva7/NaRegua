import time
from Services.RedisService import RedisService

class MockPaymentGateway:
    def __init__(self, logger):
        self.LoggerInfo = logger
        self.RedisService = RedisService('redis', 6379)

    def ProcessPayment(self, order):
        if order["PaymentType"] == 1:
            self.RedisService.Set(order['OrderId'], 1)
            self.LoggerInfo.info(f"Pedido {order['OrderId']} Processads :: Status {1}")
        else: 
            time.sleep(2)
            if order["PaymentType"] == 2 and order["CardNumber"].endswith("1"):
                self.RedisService.Set(order['OrderId'], 2)
                self.LoggerInfo.info(f"Pedido {order['OrderId']} Processado :: Status {2}")
            else:
                self.RedisService.Set(order['OrderId'], 1)
                self.LoggerInfo.info(f"Pedido {order['OrderId']} Processado :: Status {1}")