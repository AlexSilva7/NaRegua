import pika
import json
from Services.ProcessOrders import process_orders

print("Tentando conectar ao RabbitMQ...")
# credentials = pika.PlainCredentials('rbtmq1010', 'rbtmq1010')
connection = pika.BlockingConnection(pika.ConnectionParameters(
    host='rbmq', 
    credentials=pika.PlainCredentials('user', 'password'))
)

print("Aqui ...")
channel = connection.channel()
print("Conexão bem-sucedida ao RabbitMQ!")

queue_name = 'QueueOrder'
channel.queue_declare(queue=queue_name)

def processar_agendamento(ch, method, properties, body):
    pedido = json.loads(body)
    print(f"Processando agendamento: {pedido}")

    status_pagamento = process_orders(pedido)

    print(f"Enviando notifica para API principal - Pedido ID: {pedido['pedido_id']}, Status de Pagamento: {status_pagamento}")

    ch.basic_ack(delivery_tag=method.delivery_tag)

channel.basic_qos(prefetch_count=1)
channel.basic_consume(queue=queue_name, on_message_callback=processar_agendamento)

print("Aguardando agendamentos...")
channel.start_consuming()
