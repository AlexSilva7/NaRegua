import logging
from datetime import datetime

def ConfigureLogger(str_level, log_level):
    path = '/app/logs/'
    name = f"{str_level}_logger"
    log_file = f"{path}queue_{str_level}_{datetime.now().strftime('%Y%m%d')}.log"

    logger = logging.getLogger(name)
    logger.setLevel(log_level)
    
    handler = logging.FileHandler(log_file)
    handler.setLevel(log_level)
    
    formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    handler.setFormatter(formatter)
    
    logger.addHandler(handler)
    return logger