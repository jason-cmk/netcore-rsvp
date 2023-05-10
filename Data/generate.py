import random
import uuid

rnd = random.Random()

salt = ''  # input('Seed: ')


def generateId(seed):
    rnd.seed(salt + seed)
    return str(uuid.UUID(int=rnd.getrandbits(128), version=4))


file = open("data.txt", "r")

names = file.readlines()

invites = []
for name in names:
    name = name.replace('\n', '')
    invite = {'id': generateId(name), 'name': name}
    invites += [invite]

outputData = str(invites).replace("'", '"')

print('writing...')
print(outputData)

outputFile = open('data.json', 'w')

outputFile.write(outputData)

outputFile.close()
