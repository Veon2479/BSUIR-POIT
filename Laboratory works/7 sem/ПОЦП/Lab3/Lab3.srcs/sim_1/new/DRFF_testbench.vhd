----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:56:59 PM
-- Design Name: 
-- Module Name: DRFF_testbench - Behavioral
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

entity DRFF_testbench is
--  Port ( );
end DRFF_testbench;

architecture Behavioral of DRFF_testbench is
    Component DRFF is 
    Port(
        d, r, clk: in std_logic; 
        q: out std_logic
    );
    end component;
    
  function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;
   
   signal d, r, clk, q: std_logic := '0';
begin
u: DRFF port map(d, r, clk, q);

main: process
begin
    for i in 0 to 1 loop
        for j in 0 to 1 loop
            d <= to_stdlogic(i);
            r <= to_stdlogic(j);
            wait for 20 ns;
        end loop;
    end loop;
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
