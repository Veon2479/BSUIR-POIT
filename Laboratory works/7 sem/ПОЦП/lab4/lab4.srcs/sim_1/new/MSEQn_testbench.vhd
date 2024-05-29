----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 04:15:10 PM
-- Design Name: 
-- Module Name: MSEQn_testbench - Behavioral
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
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity MSEQn_testbench is
    Generic (polynom: std_logic_vector := "1100001");
end MSEQn_testbench;

architecture Behavioral of MSEQn_testbench is
    Component MSEQn is 
        Generic (pol: std_logic_vector := polynom);
        Port (
            CLK: in std_logic;
            RST: in std_logic;
            Pout: out std_logic_vector(0 to pol'high-1)
        );
    end component;
    signal clk, rst : std_logic;
    signal pout: std_logic_vector(0 to polynom'high-1);
begin
    u: MSEQn generic map(polynom) port map(clk, rst, pout);
    
    m: process
    begin
        rst <= '1';
        wait for 20 ns;
        rst <= '0';
        
        wait;
    end process;
    
    c: process
    begin
        clk <= '0';
        wait for 10 ns;
        clk <= '1';
        wait for 10 ns;
    end process;

end Behavioral;
