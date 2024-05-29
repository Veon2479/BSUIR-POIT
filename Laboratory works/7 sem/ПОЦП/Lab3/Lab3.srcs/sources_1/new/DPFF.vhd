----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 10:01:23 PM
-- Design Name: 
-- Module Name: DPFF - Behavioral
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

entity DPFF is
    Port ( D : in STD_LOGIC;
           P : in STD_LOGIC;
           Clk : in STD_LOGIC;
           Q : out STD_LOGIC);
end DPFF;

architecture Behavioral of DPFF is
    signal v : std_logic;
begin

    main: process(D, P, Clk)
    begin
        if P = '1' then 
            v <= '1'; 
        elsif rising_edge(Clk) then 
            v <= D; 
        end if;
    end process;
    Q <= v;
end Behavioral;
