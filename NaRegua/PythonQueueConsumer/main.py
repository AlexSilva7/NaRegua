import pika
import json
import logging
from Logger.ConfigureLogger import ConfigureLogger
from Services.MockPaymentGateway import MockPaymentGateway

LoggerInfo = ConfigureLogger('info', logging.INFO)
LoggerError = ConfigureLogger('error', logging.ERROR)
MockPayment = MockPaymentGateway(LoggerInfo)

def ProcessOrders(ch, method, properties, body):
    order = json.loads(body)
    LoggerInfo.info(f"Processando Pedido: {order}")

    MockPayment.ProcessPayment(order)
    ch.basic_ack(delivery_tag=method.delivery_tag)

try:
    LoggerInfo.info("Tentando Conectar ao RabbitMQ ...")
    connection = pika.BlockingConnection(pika.ConnectionParameters(
        host='rbmq', 
        credentials=pika.PlainCredentials('user', 'password'))
    )

    channel = connection.channel()
    channel.queue_declare(queue='QueueOrder')

    channel.basic_qos(prefetch_count=1)
    channel.basic_consume(queue='QueueOrder', on_message_callback=ProcessOrders)
    LoggerInfo.info("Conexão bem-sucedida ao RabbitMQ!")

    print("Aguardando Pedidos ...")
    channel.start_consuming()
except Exception as ex:
    LoggerError.error(str(ex))


