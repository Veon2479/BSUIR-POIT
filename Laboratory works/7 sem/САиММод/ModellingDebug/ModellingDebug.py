from random import random
import math
import numpy as np

def get_time(l):
    r = random()
    return (-1 / l) * math.log(r)


def is_zero(f):
    return (f < 10e-12)

def get_gen_time(l):
    # return np.random.exponential(l)
    return get_time(l)

class sim:

    debug = False

    # params
    lin = 1.5
    lout = 2
    avgw = 0.4

    ls = [lin, 1 / avgw, lout]

    # state
    time = [0.0] * 3 # [0] is input, [1] is warm-up, [2] is out element

    qlen = 0
    isCold = True
    isEmpty = True
    isWarming = False

    # stats
    generated = 0
    finished = 0
    elapsed = 0.0

    avgq = 0
    avgs = 0
    iters = 0

    # def __init__(self):
        # self.time[0] = get_gen_time(self.ls[0])

    def print_state(self):
        if (self.debug):
            print()
            print(f'times: {self.time}')
            print(f'qlen: {self.qlen}')
            print(f'isCold {self.isCold}')
            print(f'isEmpty {self.isEmpty}')
            print(f'isWarming {self.isWarming}')
            print(f'generated {self.generated}')
            print(f'finished {self.finished}')
            print(f'elapsed {self.elapsed}')


    def run_sim(self, iterations):

        for c in range(iterations):

            min_t = math.inf

            for i in range(len(self.time)):
                # if is_zero(time[i]):
                #     time[i] = get_time(ls[i])
                if (min_t > self.time[i] and self.time[i] != 0):
                    min_t = self.time[i]

            if (min_t == math.inf):
                min_t = 0.0
            self.elapsed += min_t

            for i in range(len(self.time) - 1, -1, -1):
                self.time[i] -= min_t
                if (self.time[i] < 0):
                    self.time[i] = 0

            # update state

            # computing element
            if (is_zero(self.time[2])):
                if (self.isEmpty == False):
                    self.isEmpty = True
                    self.finished += 1
                else:
                    self.isCold = True

            self.print_state()


            # warming element
            if (is_zero(self.time[1])):
                # if (self.isWarming):
                #     self.isWarming = False
                #     self.isCold = False

                if (self.qlen != 0):
                    if (self.isCold):
                        if (self.isWarming == True):
                            self.isWarming = False
                            self.isCold = False
                            self.qlen -= 1
                            self.time[2] = get_time(self.ls[2])
                            self.isEmpty = False
                        else:
                            self.isWarming = True
                            self.time[1] = get_time(self.ls[1])
                    else:
                        if (self.isEmpty):
                            self.qlen -= 1
                            self.isEmpty = False
                            self.time[2] = get_time(self.ls[2])
            self.print_state()


            # generator
            if (is_zero(self.time[0])):
                if (self.isCold):
                    self.qlen += 1
                    if (self.isWarming == False):
                        self.isWarming = True
                        self.time[1] = get_time(self.ls[1])
                else:
                    if (self.isEmpty):
                        self.time[2] = get_time(self.ls[2])
                        self.isEmpty = False
                    else:
                        self.qlen += 1
                self.generated += 1
                self.time[0] = get_gen_time(self.ls[0])

            self.avgq += self.qlen

            self.avgs += self.qlen
            if not self.isEmpty:
                self.avgs += 1

            self.print_state()
            if (self.debug):
                print("=========================================")

        self.iters += iterations

        print(f'Time elapsed: {self.elapsed}')
        print()
        print(f'Generated reqs: {self.generated}')
        print(f'Finished  reqs: {self.finished}')
        print(f'Computing reqs: {self.generated-self.finished}')
        print()
        print(f'Queue length: {self.qlen}')
        print()
        print(f'Avg Sys l: {self.avgs/ self.iters}')
        print(f'Avg s ttl: {self.elapsed / self.generated}')
        print()
        print(f'Avg Queue: {self.avgq / self.iters}')
        print(f'Avg q ttl: {self.elapsed / self.avgq}')
        print()

s = sim()
s.run_sim(1000000)


# from random import random
# import math
#
# def get_time(l):
#     r = random()
#     return (-1 / l) * math.log(r)
#
# def is_zero(f):
#     return (f < 10e-12)
#
# class Sim:
#     def __init__(self):
#         self.debug = False
#         self.lin = 1.5  # Входная интенсивность
#         self.lout = 2  # Интенсивность обслуживания
#         self.avgw = 0.4  # Среднее время "разогрева"
#
#         self.ls = [self.lin, 1 / self.avgw, self.lout]
#
#         self.time = [0.0] * 3  # [0] - время поступления, [1] - время "разогрева", [2] - время обслуживания
#         self.qlen = 0
#         self.isCold = True
#         self.isEmpty = True
#         self.isWarming = False
#
#         self.generated = 0
#         self.finished = 0
#         self.elapsed = 0.0
#
#     def get_gen_time(self, l):
#         return get_time(l)
#
#     def print_state(self):
#         if self.debug:
#             print(f'times: {self.time}')
#             print(f'qlen: {self.qlen}')
#             print(f'isCold: {self.isCold}')
#             print(f'isEmpty: {self.isEmpty}')
#             print(f'isWarming: {self.isWarming}')
#             print(f'generated: {self.generated}')
#             print(f'finished: {self.finished}')
#             print(f'elapsed: {self.elapsed}')
#             print("-------------------------------")
#
#     def run_sim(self, iterations):
#         for _ in range(iterations):
#             min_t = min(filter(lambda t: t > 0, self.time), default=0.0)
#             self.elapsed += min_t
#
#             for i in range(len(self.time)):
#                 self.time[i] -= min_t
#                 if self.time[i] < 0:
#                     self.time[i] = 0
#
#             if is_zero(self.time[2]):
#                 if not self.isEmpty:
#                     self.isEmpty = True
#                     self.finished += 1
#                 else:
#                     self.isCold = True
#
#             if is_zero(self.time[1]):
#                 if self.qlen != 0:
#                     if self.isCold:
#                         if self.isWarming:
#                             self.isWarming = False
#                             self.isCold = False
#                             self.qlen -= 1
#                             self.time[2] = get_time(self.ls[2])
#                             self.isEmpty = False
#                         else:
#                             self.isWarming = True
#                             self.time[1] = self.get_gen_time(self.ls[1])
#
#             if is_zero(self.time[0]):
#                 if self.isCold:
#                     self.qlen += 1
#                     if not self.isWarming:
#                         self.isWarming = True
#                         self.time[1] = self.get_gen_time(self.ls[1])
#                 else:
#                     if self.isEmpty:
#                         self.time[2] = get_time(self.ls[2])
#                         self.isEmpty = False
#                     else:
#                         self.qlen += 1
#                 self.generated += 1
#                 self.time[0] = self.get_gen_time(self.ls[0])
#
#             if self.debug:
#                 self.print_state()
#
#         print(f'Time elapsed: {self.elapsed}')
#         print(f'Generated reqs: {self.generated}')
#         print(f'Finished  reqs: {self.finished}')
#         print(f'Computing reqs: {self.generated - self.finished}')
#         print(f'Queue length: {self.qlen}')
#
# s = Sim()
# s.run_sim(100000)


# import numpy as np
#
# class SimulatedQueue:
#     def __init__(self, arrival_rate, service_rate, warmup_mean):
#         self.arrival_rate = arrival_rate
#         self.service_rate = service_rate
#         self.warmup_mean = warmup_mean
#
#         self.queue = []  # очередь
#         self.current_time = 0.0  # текущее время
#         self.service_time = 0.0  # время обслуживания
#         self.warmup_time = np.random.exponential(warmup_mean)  # время разогрева
#
#         self.total_requests = 0  # общее количество заявок
#         self.serviced_requests = 0  # количество обслуженных заявок
#         self.total_time_in_queue = 0.0  # общее время в очереди
#         self.total_time_in_system = 0.0  # общее время в системе
#         self.max_queue_length = 0  # максимальная длина очереди
#
#     def run_simulation(self, total_time):
#         while self.current_time < total_time:
#             # генерируем время до следующей заявки и времени обслуживания
#             interarrival_time = np.random.exponential(1 / self.arrival_rate)
#             service_time = np.random.exponential(1 / self.service_rate)
#
#             # обновляем текущее время
#             self.current_time += interarrival_time
#
#             if self.current_time >= total_time:
#                 break
#
#             # проверяем, нужно ли разогревить систему
#             if self.warmup_time > 0:
#                 self.warmup_time -= interarrival_time
#
#             if self.warmup_time <= 0:
#                 # заявка проходит разогрев и попадает в обслуживание или очередь
#                 if self.service_time <= 0:
#                     self.service_time = service_time
#                     self.serviced_requests += 1
#                 else:
#                     self.queue.append(self.current_time)
#                     self.total_time_in_queue += len(self.queue) * interarrival_time
#
#                     # обновляем максимальную длину очереди
#                     if len(self.queue) > self.max_queue_length:
#                         self.max_queue_length = len(self.queue)
#
#             # обновляем время обслуживания
#             if self.service_time > 0:
#                 self.service_time -= interarrival_time
#                 self.total_time_in_system += interarrival_time
#
#             self.total_requests += 1
#
#         # вычисляем средние значения
#         avg_time_in_queue = self.total_time_in_queue / self.total_requests
#         avg_time_in_system = self.total_time_in_system / self.serviced_requests
#         avg_queue_length = self.total_time_in_queue / total_time
#         return avg_time_in_queue, avg_time_in_system, avg_queue_length
#
# # Пример использования
# sim = SimulatedQueue(arrival_rate=1.5, service_rate=2, warmup_mean=0.4)
# avg_time_in_queue, avg_time_in_system, avg_queue_length = sim.run_simulation(total_time=1000000)
#
# print(f"Среднее время в очереди: {avg_time_in_queue}")
# print(f"Среднее время в системе: {avg_time_in_system}")
# print(f"Средняя длина очереди: {avg_queue_length}")
