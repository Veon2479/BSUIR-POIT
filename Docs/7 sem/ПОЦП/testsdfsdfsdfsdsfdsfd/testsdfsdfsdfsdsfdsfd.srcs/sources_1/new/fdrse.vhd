----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2022 10:30:44
-- Design Name: 
-- Module Name: fdrse - Behavioral
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

entity fdrse is
    Port ( R : in STD_LOGIC;
           S : in STD_LOGIC;
           CE : in STD_LOGIC;
           D : in STD_LOGIC;
           C : in STD_LOGIC;
           Q : out STD_LOGIC);
end fdrse;

architecture Behavioral of fdrse is

begin
p1: process (R,S,CE,C)
begin
    if (rising_edge(C))then
    if (R = '1')then 
        Q<='0';
    elsif(S='1') then
        Q<='1';
    elsif (CE='1') then
        Q<=D;
        end if;
    end if;
end process;
end Behavioral;
