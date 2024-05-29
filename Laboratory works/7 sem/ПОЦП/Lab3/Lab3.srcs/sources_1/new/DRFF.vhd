----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:54:24 PM
-- Design Name: 
-- Module Name: DRFF - Behavioral
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

entity DRFF is
    Port(
        d, r, clk: in std_logic; 
        q: out std_logic
    );
end DRFF;

architecture Behavioral of DRFF is
    signal v : std_logic;
begin
    main: process(d, r, clk)
    begin
        if r = '1' then 
            v <= '0'; 
        elsif rising_edge(clk) then 
            v <= d; 
        end if;
    end process;
    q <= v;
end Behavioral;
