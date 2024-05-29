----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:11:07 PM
-- Design Name: 
-- Module Name: DFF - Behavioral
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

entity DFF is
    Port(
        d, clk: in std_logic; 
        q: out std_logic
    );
end DFF;

architecture Behavioral of DFF is
    signal v : std_logic;
begin
    Main: process(d, clk)
    begin
        if rising_edge(clk) then
            v <= d;
        end if;
    end process;

    Q <= v;

end Behavioral;
