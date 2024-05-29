----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 03:36:23 PM
-- Design Name: 
-- Module Name: JC_testbench - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity JC_testbench is
    Generic (i : integer := 2);
end JC_testbench;

architecture Behavioral of JC_testbench is
    Component JC is 
        Generic (i : integer := 2);
        Port (
            CLK: in std_logic;
            RST: in std_logic;
            LS: in std_logic; --at '0' - setting init state 
            Pin: in std_logic_vector(0 to 2**i-1);
            Pout: out std_logic_vector(0 to 2**i-1)
        );    
    end component;
    signal clk, rst, ls : std_logic := '0';
    signal inp, outp : std_logic_vector(0 to 2**i-1) := (others => '0');
begin
    u: JC port map(clk, rst, ls, inp, outp);
    
    main: process 
    begin 
        rst <= '1'; wait for 10 ns;
        rst <= '0'; 
        ls <= '0'; wait for 10 ns;
        
        inp <= "1111"; wait for 20 ns;
        
        ls <= '1'; wait for 20 ns;
        
        wait;
    end process;
    
    cl: process
    begin 
        clk <= '0';
        wait for 10 ns;
        clk <= '1';
        wait for 10 ns;
    end process;

end Behavioral;
