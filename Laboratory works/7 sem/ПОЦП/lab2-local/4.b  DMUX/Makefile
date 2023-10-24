GHDL = ghdl
SRC_FILES = ../lib/*.vhd ./*.vhd
ENTITY = Dmux_testbench
VCD_FILE = wave.vcd
LOG_FILE = log.txt

all: build run

build: analyze elaborate

analyze:
	@$(GHDL) -a $(SRC_FILES)

elaborate:
	@$(GHDL) -e $(ENTITY)

run-quite:
	@$(GHDL) -r $(ENTITY) --vcd=${VCD_FILE}&> ${LOG_FILE}
run:
	@$(GHDL) -r $(ENTITY) --vcd=${VCD_FILE}

clean:
	@$(GHDL) --clean
	@rm -f *.o $(ENTITY) *.cf ${LOG_FILE} ${VCD_FILE}
