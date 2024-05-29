----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:18:16 PM
-- Design Name: 
-- Module Name: DEFF - Behavioral
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

entity DEFF is
    Port(
        d, e, clk: in std_logic; 
        q: out std_logic
    );

end DEFF;

architecture Behavioral of DEFF is
    signal v : std_logic;
begin
    Main: process(e, d, clk)
    begin
        if e = '1' then
            if rising_edge(clk) then 
                v <= d;
            end if;
        end if;
    end process;

    Q <= v;

end Behavioral;
