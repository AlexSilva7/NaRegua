
def process_orders(pedido):
    if pedido["tipo_pagamento"] == "credito" and pedido["numero_cartao"].endswith("1"):
        return "negado"
    else:
        return "aprovado"
