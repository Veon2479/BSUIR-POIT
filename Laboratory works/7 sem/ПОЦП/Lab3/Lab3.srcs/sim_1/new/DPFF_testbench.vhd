----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 10:02:33 PM
-- Design Name: 
-- Module Name: DPFF_testbench - Behavioral
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

entity DPFF_testbench is
--  Port ( );
end DPFF_testbench;

architecture Behavioral of DPFF_testbench is
  function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;
   Component DPFF is 
   Port ( D : in STD_LOGIC;
           P : in STD_LOGIC;
           Clk : in STD_LOGIC;
           Q : out STD_LOGIC);
   end component;
   
   signal d, p, clk, q: std_logic := '0';
   
begin
u: DPFF port map(d, p, clk, q);

m: process
begin
    for i in 0 to 1 loop 
        for j in 0 to 1 loop
            d <= to_stdlogic(i); 
            p <= to_stdlogic(j);
            wait for 20 ns;
        end loop;
    end loop;
end process;

cl: process
begin
    clk <= '0';
    wait for 10 ns;
    clk <= '1';
    wait for 10 ns;
end process;

end Behavioral;
