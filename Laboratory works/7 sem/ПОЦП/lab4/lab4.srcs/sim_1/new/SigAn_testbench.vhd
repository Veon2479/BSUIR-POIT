----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 07:34:00 PM
-- Design Name: 
-- Module Name: SigAn_testbench - Behavioral
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
use IEEE.NUMERIC_STD.ALL;
use IEEE.MATH_REAL.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity SigAn_testbench is
    Generic (polynom : std_logic_vector := "1100001");
end SigAn_testbench;

architecture Behavioral of SigAn_testbench is

    Component SigAn is 
        Generic (pol: std_logic_vector := polynom);
        Port ( Clk : in STD_LOGIC;
               Din : in STD_LOGIC;
               Rst : in STD_LOGIC;
               Dout : out STD_LOGIC_VECTOR (0 to pol'high-1));
    end component;
    
    signal clk, din, rst : std_logic;
    signal dout: STD_LOGIC_VECTOR (0 to polynom'high-1);
    
begin
    u: SigAn generic map (polynom) port map(clk, din, rst, dout);
    
    m: process
        variable seed1, seed2 : positive;
        variable rand : real := 0.0;
        
        variable test : std_logic_vector(0 to 8) := "110001101";
    begin
        rst <= '1'; wait for 15 ns;
        rst <= '0'; wait for 5 ns;
--        seed1 := 37;
--        seed2 := 59;
    

--        for i in 0 to 100 loop
--            UNIFORM(seed1, seed2, rand);
--            if (rand >= 0.5) then 
--                din <= '1';
--            else
--                din <= '0';
--            end if;
--            wait for 20 ns;
--        end loop;
        for i in 0 to test'length-1 loop
            din <= test(i);
            wait for 20 ns;
        end loop;

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
