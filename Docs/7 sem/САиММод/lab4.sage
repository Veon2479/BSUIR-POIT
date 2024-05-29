from sage.all import *

u, l, v = var('u, l, v')

count = 10
p0 = var(['p0{}'.format(i) for i in range(0, count + 1)])
p1 = var(['p1{}'.format(i) for i in range(0, count + 1)])

eqs = []

eqs.append( u==2 )
eqs.append( l==1.5)
eqs.append( v==1/0.4 )

# sum of p_i == 1
eqs.append( sum(p0[i] for i in range(count+1))+sum(p1[i] for i in range(1, count+1))==1 )

# left edge
eqs.append( l*p0[0]==u*p1[1] )
eqs.append( (v+l)*p0[1]==l*p0[0] )
eqs.append( (u+l)*p1[1]==v*p0[1]+u*p1[2] )

# right edge
eqs.append( v*p0[count]==l*p0[count-1] )
eqs.append( u*p1[count]==v*p0[count]+l*p1[count-1] )

# middle-equations
for i in range(2, count):
    eqs.append( (l+v)*p0[i]==l*p0[i-1] )
    eqs.append( (u+l)*p1[i]==l*p1[i-1]+v*p0[i]+u*p1[i+1] )

print("Equations:")
for i in eqs:
    print(f'    {i}')
print()

s = solve(eqs, u, l, v,
                 p0[0],p0[1],p0[2],p0[3],p0[4],p0[5],p0[6],p0[7],p0[8],p0[9],p0[10],
                 p1[1],p1[2],p1[3],p1[4],p1[5],p1[6],p1[7],p1[8],p1[9],p1[10])

p0s = []
p1s = [0]

for sl in s:
    print("Solution:")
    for i in sl:
        print(f'    {i}')

    u = sl[0].rhs()
    l = sl[1].rhs()
    v = sl[2].rhs()

    for i in range(count + 1):
        p0s.append(sl[3 + i].rhs())
    # print(p0s)


    for i in range(1, count + 1):
        p1s.append(sl[3 + count + i].rhs())
    # print(p1s)

    print()

    z = 0
    for i in range(1, count + 1):
        z += i * (p0s[i] + p1s[i])
    print(f'    Average req count is {N(z)}')
    print(f'    Average req ttl   is {N(z / l)}')

    print()

    r = 0
    for i in range(1, count):
        r += i * (p0s[i] + p1s[i + 1])
    print(f'    Average queue len is {N(r)}')
    print(f'    Average queue ttl is {N(r / l)}')
